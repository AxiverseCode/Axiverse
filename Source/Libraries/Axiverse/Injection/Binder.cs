using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Injection
{
    /// <summary>
    /// Represents a binding between a <see cref="Type"/> and a <see cref="IBindingProvider"/>.
    /// </summary>
    public class Binder
    {
        /// <summary>
        /// Creates on object binding to the given type. All public properties with a setter and
        /// non-readonly fields which have a <see cref="BindAttribute"/> attribute set will have
        /// bindings created. Derived types bound will only be bounds based on the fields and
        /// properties of the given types and may not represent the full set of bindings on that
        /// type. Relevant fields and properties will be cached.
        /// </summary>
        /// <param name="type"></param>
        public Binder(Type type)
        {
            var properties = type.GetProperties(BindingFlags.SetProperty | BindingFlags.Public);
            foreach (var property in properties)
            {
                var key = Key.From(property.PropertyType, property.Name);
                this.properties.Add(key, property);
            }
            
            var fields = type.GetFields(BindingFlags.Public);
            foreach (var field in fields)
            {
                if (!field.IsInitOnly)
                {
                    var key = Key.From(field.FieldType, field.Name);
                    this.fields.Add(key, field);
                }
            }
        }

        /// <summary>
        /// Binds all bindings to the given class object. This method will not work on structs.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="bindings"></param>
        public void SetValues(object obj, IBindingProvider bindings)
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
        public void SetValues<T>(ref T obj, IBindingProvider bindings)
            where T : struct
        {
            foreach (var property in properties)
            {
                if (bindings.TryGetValue(property.Key, out var value))
                {
                    property.Value.SetValue(obj, value);
                }
            }

            var reference = __makeref(obj);
            foreach (var field in fields)
            {
                if (bindings.TryGetValue(field.Key, out var value))
                {
                    field.Value.SetValueDirect(reference, value);
                }
            }
        }

        public static void Bind(object obj, IBindingProvider bindings)
        {
            var type = obj.GetType();
            Contract.Requires<InvalidCastException>(obj.GetType().IsClass);

            var properties = type.GetProperties(BindingFlags.SetProperty | BindingFlags.Public);
            foreach (var property in properties)
            {
                var key = Key.From(property.PropertyType, property.Name);
                if (!bindings.TryGetValue(key, out var value))
                {
                    throw new MissingFieldException($"Missing binding for {key}.");
                }
                property.SetValue(obj, value);
            }

            var fields = type.GetFields(BindingFlags.Public);
            foreach (var field in fields)
            {
                if (!field.IsInitOnly)
                {
                    var key = Key.From(field.FieldType, field.Name);
                    if (bindings.TryGetValue(key, out var value))
                    {
                        throw new MissingFieldException($"Missing binding for {key}.");
                    }
                    field.SetValue(obj, value);
                }
            }
        }

        public static void Bind(ref object obj, IBindingProvider bindings)
        {
            var type = obj.GetType();
            Requires.That(type.IsValueType || type.IsClass);

            var properties = type.GetProperties(BindingFlags.SetProperty | BindingFlags.Public);
            foreach (var property in properties)
            {
                var key = Key.From(property.PropertyType, property.Name);
                if (!bindings.TryGetValue(key, out var value))
                {
                    throw new MissingFieldException($"Missing binding for {key}.");
                }
                property.SetValue(obj, value);
            }

            var reference = __makeref(obj);
            var fields = type.GetFields(BindingFlags.Public);
            foreach (var field in fields)
            {
                if (!field.IsInitOnly)
                {
                    var key = Key.From(field.FieldType, field.Name);
                    if (bindings.TryGetValue(key, out var value))
                    {
                        throw new MissingFieldException($"Missing binding for {key}.");
                    }
                    field.SetValueDirect(reference, value);
                }
            }
        }

        /// <summary>
        /// Activates a new instance of the type with the injector parameters bound from the
        /// <see cref="IBindingProvider"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bindings"></param>
        /// <returns></returns>
        public static T Activate<T>(IBindingProvider bindings)
        {
            var type = typeof(T);

            var constructors = type.GetConstructors(BindingFlags.Public);
            ConstructorInfo injectConstructor = type.GetConstructor(Type.EmptyTypes);

            // find best constructor
            foreach (var constructor in constructors)
            {
                if (constructor.GetCustomAttributes<InjectAttribute>().Count() > 0)
                {
                    Requires.That<AmbiguousMatchException>(injectConstructor == null);
                }
            }

            if (injectConstructor == null)
            {
                throw new MissingMethodException();
            }

            // initialize injected fields and properties
            var value = FormatterServices.GetUninitializedObject(type);
            Bind(ref value, bindings);

            var parameters = injectConstructor.GetParameters();
            var parameterValues = new object[parameters.Length];
            // find attributes with binding.
            for (int i = 0; i < parameters.Length; i++)
            {
                if (bindings.TryGetValue(Key.From(parameters[i]), out var parameterValue))
                {
                    parameterValues[i] = parameterValue;
                }
                else
                {
                    throw new MissingFieldException();
                }
            }

            injectConstructor.Invoke(value, parameterValues);

            throw new NotImplementedException();
        }

        private readonly Dictionary<Key, FieldInfo> fields = new Dictionary<Key, FieldInfo>();
        private readonly Dictionary<Key, PropertyInfo> properties = new Dictionary<Key, PropertyInfo>();
    }
}
