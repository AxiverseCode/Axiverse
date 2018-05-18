using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILoader<T>
    {
        /// <summary>
        /// Gets the array of extensions supported by the loader.
        /// </summary>
        string[] Extensions { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        T Load(Stream stream, LoadContext context);
    }
}
