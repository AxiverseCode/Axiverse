using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources.New
{
    /// <summary>
    /// Represents an stream to object deserializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDeserializer<T>
    {
        /// <summary>
        /// Tries to deserialize the given stream into a typed object.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryDeserialize(Stream stream, out T value);
    }
}
