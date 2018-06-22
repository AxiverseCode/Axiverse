using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Injection;

namespace Axiverse
{
    /// <summary>
    /// Functions for preconditions.
    /// </summary>
    public static class Requires
    {
        /// <summary>
        /// Throws an exception of the conditions are not met.
        /// </summary>
        /// <param name="condition"></param>
        public static void That(bool condition)
        {
            if (!condition)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Throws an exception of the conditions are not met.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void That(bool condition, string format, params object[] args)
        {
            if (!condition)
            {
                throw new Exception(string.Format(format, args));
            }
        }

        /// <summary>
        /// Throws an exception of the conditions are not met.
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="condition"></param>
        public static void That<TException>(bool condition) where TException : Exception, new()
        {
            if (!condition)
            {
                throw new TException();
            }
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> if the object is disposed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="disposable"></param>
        public static void IsNotDisposed<T>(T disposable) where T: ITrackedDisposable
        {
            if (disposable.IsDisposed)
            {
                throw new ObjectDisposedException(nameof(T));
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidCastException"/> if the object cannot be assigned to key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static T AssignableFrom<T>(T value)
        {
            if (!typeof(T).IsAssignableFrom(value.GetType()))
            {
                throw new InvalidCastException($"Cannot assign {value.GetType().Name} to type {typeof(T).Name}");
            }
            return value;
        }

        /// <summary>
        /// Throws an <see cref="InvalidCastException"/> if the object cannot be assigned to key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static T AssignableFrom<T>(Key key, T value)
        {
            if (!key.IsAssignableFrom(value))
            {
                throw new InvalidCastException($"Cannot assign {value.GetType().Name} to key {key}");
            }
            return value;
        }

        /// <summary>
        /// Throws an <see cref="InvalidCastException"/> if the type T cannot be assigned to key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public static void AssignableFrom<T>(Key key)
        {
            if (!key.IsAssignableFrom(typeof(T)))
            {
                throw new InvalidCastException($"Cannot assign {typeof(T).Name} to key {key}");
            }
        }
    }
}
