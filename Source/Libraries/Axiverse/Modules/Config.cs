using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Modules
{
    public class Config<T>
    {
        /// <summary>
        /// The value of the configuration.
        /// </summary>
        public T Value { get; internal set; }


        /// <summary>
        /// Casts the <see cref="Config{T}"/> to the value it represents.
        /// </summary>
        /// <param name="config"></param>
        public static implicit operator T(Config<T> config)
        {
            return config.Value;
        }
    }
}
