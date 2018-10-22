using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class CachedStore
    {
        // a cached store has both a source store (needs to be able to read) and a cache store
        // (needs both read and write). When reading it looks in the cache first, perhaps looking
        // at the last modified tags from the source store? If found and up to date, it uses that.
        // otherwise it reads the data from the source store. Often this is used with a slower
        // source store (e.g. a network based store) and a fast cache store (e.g. file store).
    }
}
