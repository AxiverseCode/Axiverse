using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse
{
    /// <summary>
    /// Functions for preconditions.
    /// </summary>
    public static class Preconditions
    {
        /// <summary>
        /// Throws an exception of the conditions are not met.
        /// </summary>
        /// <param name="condition"></param>
        public static void Requires(bool condition)
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
        public static void Requires(bool condition, string format, params object[] args)
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
        public static void Requires<TException>(bool condition) where TException : Exception, new()
        {
            if (!condition)
            {
                throw new TException();
            }
        }
    }
}
