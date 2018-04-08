using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Injection
{
    /// <summary>
    /// Represents a binding between a <see cref="Type"/> and a <see cref="IBindingProvider"/>.
    /// </summary>
    public class ObjectBinding
    {
        /// <summary>
        /// Creates on object binding to the given type. All public properties with a setter and
        /// non-readonly fields which have a <see cref="BindAttribute"/> attribute set will have
        /// bindings created. Derived types bound will only be bounds based on the fields and
        /// properties of the given types and may not represent the full set of bindings on that
        /// type.
        /// </summary>
        /// <param name="type"></param>
        public ObjectBinding(Type type)
        {
            var properties = type.GetProperties(BindingFlags.SetProperty | BindingFlags.Public);
            foreach (var property in properties)
            {
                var key = Key.From(property.PropertyType, property.Name);
            }
            
            var fields = type.GetFields(BindingFlags.Public);
            foreach (var field in fields)
            {
                if (!field.IsInitOnly)
                {
                    var key = Key.From(field.FieldType, field.Name);
                }
            }
        }

        /// <summary>
        /// Binds all bindings to the given class object. This method will not work on structs.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="bindings"></param>
        public void Bind(object obj, IBindingProvider bindings)
        {
            Contract.Requires<InvalidCastException>(obj.GetType().IsValueType == false);

            foreach (var property in properties)
            {
                if (bindings.TryGetValue(property.Key, out var value))
                {
                    property.Value.SetValue(obj, value);
                }
            }

            foreach (var field in fields)
            {
                if (bindings.TryGetValue(field.Key, out var value))
                {
                    field.Value.SetValue(obj, value);
                }
            }
        }

        /// <summary>
        /// Binds all bindings to the given reference object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="bindings"></param>
        public void Bind(ref object obj, IBindingProvider bindings)
        {
            foreach (var property in properties)
            {
                if (bindings.TryGetValue(property.Key, out var value))
                {
                    property.Value.SetValue(obj, value);
                }
            }

            TypedReference reference = __makeref(obj);
            foreach (var field in fields)
            {
                if (bindings.TryGetValue(field.Key, out var value))
                {
                    field.Value.SetValueDirect(reference, value);
                }
            }
        }

        private readonly Dictionary<Key, FieldInfo> fields = new Dictionary<Key, FieldInfo>();
        private readonly Dictionary<Key, PropertyInfo> properties = new Dictionary<Key, PropertyInfo>();
    }
}
