using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Axiverse.Injection
{
    /// <summary>
    /// Represents a binding between a <see cref="Type"/> and a <see cref="IBindingProvider"/>.
    /// </summary>
    public static class Binder
    {
        /// <summary>
        /// Binds the fields and properties on a class.
        /// 
        /// By default, fields with the <see cref="BindAttribute"/> applied will be set. This
        /// includes readonly fields. If <paramref name="forceAll"/> is set then all public fields
        /// will be bound from the injector.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="bindings"></param>
        /// <param name="forceAll"></param>
        public static void Bind<T>(ref T obj, IBindingProvider bindings, bool forceAll = false)
        {
            var type = obj.GetType();
            var propertyEntires = GetBindingProperties(type, forceAll);
            foreach (var propertyEntry in propertyEntires)
            {
                propertyEntry.Value.SetValue(obj, bindings[propertyEntry.Key]);
            }

            var reference = __makeref(obj);
            var fieldEntries = GetBindingFields(type, forceAll);
            foreach (var fieldEntry in fieldEntries)
            {
                fieldEntry.Value.SetValueDirect(reference, bindings[fieldEntry.Key]);
            }
        }

        /// <summary>
        /// Activates a new instance of the type with the fields and properties bound and then the
        /// constructor called afterwards.
        /// <see cref="IBindingProvider"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bindings"></param>
        /// <param name="forceAll"></param>
        /// <returns></returns>
        public static T Activate<T>(IBindingProvider bindings, bool forceAll = false)
        {
            var type = typeof(T);
            return (T)Activate(type, bindings, forceAll);
        }

        /// <summary>
        /// Activates a new instance of the type with the fields and properties bound and then the
        /// constructor called afterwards.
        /// <see cref="IBindingProvider"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bindings"></param>
        /// <param name="forceAll"></param>
        /// <returns></returns>
        public static object Activate(Type type, IBindingProvider bindings, bool forceAll = false)
        {
            ConstructorInfo constructor = GetConstructor(type);

            var value = FormatterServices.GetUninitializedObject(type);
            Bind(ref value, bindings, forceAll);

            var parameterInfos = constructor.GetParameters();
            var parameters = new object[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                parameters[i] = bindings[GetKey(parameterInfos[i])];
            }
            constructor.Invoke(value, parameters);

            return value;
        }

        /// <summary>
        /// Gets the constructor to use for activation.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ConstructorInfo GetConstructor(Type type)
        {
            ConstructorInfo result = null;

            foreach (var constructorInfo in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public))
            {
                if (constructorInfo.GetCustomAttribute<InjectAttribute>(false) != null)
                {
                    Requires.That<AmbiguousMatchException>(result == null);
                    result = constructorInfo;
                }
            }
            
            return result ?? type.GetConstructor(Array.Empty<Type>()) ?? throw new MissingMethodException();
        }

        /// <summary>
        /// Gets the field for binding and their associated keys.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="forceAll"></param>
        /// <returns></returns>
        public static Dictionary<Key, FieldInfo> GetBindingFields(Type type, bool forceAll = false)
        {
            var result = new Dictionary<Key, FieldInfo>();
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;

            if (!forceAll)
            {
                bindingFlags |= BindingFlags.NonPublic;
            }

            foreach (var fieldInfo in type.GetFields(bindingFlags))
            {
                if (forceAll || fieldInfo.GetCustomAttribute<BindAttribute>() != null)
                {
                    result.Add(GetKey(fieldInfo), fieldInfo);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the properties for binding and their associated keys.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="forceAll"></param>
        /// <returns></returns>
        public static Dictionary<Key, PropertyInfo> GetBindingProperties(Type type, bool forceAll = false)
        {
            var result = new Dictionary<Key, PropertyInfo>();
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty;
            
            if (!forceAll)
            {
                bindingFlags |= BindingFlags.NonPublic;
            }

            foreach (var proeprtyInfo in type.GetProperties(bindingFlags))
            {
                if (forceAll || proeprtyInfo.GetCustomAttribute<BindAttribute>() != null)
                {
                    result.Add(GetKey(proeprtyInfo), proeprtyInfo);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the key to be used for a field.
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static Key GetKey(FieldInfo fieldInfo)
        {
            return Key.From(fieldInfo.FieldType, fieldInfo.GetCustomAttributes());
        }

        /// <summary>
        /// Gets the key to be used for a property.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static Key GetKey(PropertyInfo propertyInfo)
        {
            return Key.From(propertyInfo.PropertyType, propertyInfo.GetCustomAttributes());
        }

        /// <summary>
        /// Gets the key to be used for a parameter.
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        public static Key GetKey(ParameterInfo parameterInfo)
        {
            return Key.From(parameterInfo.ParameterType, parameterInfo.GetCustomAttributes());
        }
    }
}
