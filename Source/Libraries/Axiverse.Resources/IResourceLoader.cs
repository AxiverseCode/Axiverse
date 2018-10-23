using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    /// <summary>
    /// Defines methods to load resources.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResourceLoader<T>
    {
        /// <summary>
        /// Gets the name of the loader.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the preferred extensions which this loader is compatable with.
        /// </summary>
        string[] Extensions { get; }

        /// <summary>
        /// Loads the resource with the given stream.
        /// </summary>
        /// <param name="stream">The stream to load.</param>
        /// <param name="value">
        /// When this method returns, contains the value of the loaded resource, if the stream
        /// could be loaded; otherwise the default value for the type of the
        /// <paramref name="value"/> parameter. This parameter is passed uninitialzed.
        /// </param>
        /// <returns>
        /// <code>true</code> if the <see cref="IResourceLoader{T}"/> was able to load the resource with
        /// the specified stream; otherwise, <code>false</code>.
        /// </returns>
        bool TryLoad(Stream stream, out T value);
    }
}
