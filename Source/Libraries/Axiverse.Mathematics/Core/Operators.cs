using System;
using System.Linq.Expressions;

namespace Axiverse
{
    /// <summary>
    /// Generic operator usage.
    /// </summary>
    /// <remarks>
    /// We cannot use operator on generics so we have to use this bridge class in order to enable
    /// that.
    /// </remarks>
    public static class Operators
    {
        /// <summary>
        /// Adds two objects together.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T Add<T>(T a, T b)
        {
            return Operators<T>.add(a, b);
        }

        /// <summary>
        /// Substracts two objects together.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T Subtract<T>(T a, T b)
        {
            return Operators<T>.subtract(a, b);
        }

        /// <summary>
        /// Multiplies two objects together.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T Multiply<T>(T a, T b)
        {
            return Operators<T>.multiply(a, b);
        }

        /// <summary>
        /// Divides two objects together.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T Divide<T>(T a, T b)
        {
            return Operators<T>.divide(a, b);
        }
    }

    /// <summary>
    /// Static object container for functions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static class Operators<T>
    {
        public static readonly Func<T, T, T> add;
        public static readonly Func<T, T, T> subtract;
        public static readonly Func<T, T, T> multiply;
        public static readonly Func<T, T, T> divide;

        static Operators()
        {
            var a = Expression.Parameter(typeof(T), "a");
            var b = Expression.Parameter(typeof(T), "b");

            var addBody = Expression.Add(a, b);
            add = Expression.Lambda<Func<T, T, T>>(addBody, a, b).Compile();

            var subtractBody = Expression.Subtract(a, b);
            subtract = Expression.Lambda<Func<T, T, T>>(subtractBody, a, b).Compile();

            var multiplyBody = Expression.Multiply(a, b);
            multiply = Expression.Lambda<Func<T, T, T>>(multiplyBody, a, b).Compile();

            var divideBody = Expression.Divide(a, b);
            divide = Expression.Lambda<Func<T, T, T>>(divideBody, a, b).Compile();

        }
    }
}
