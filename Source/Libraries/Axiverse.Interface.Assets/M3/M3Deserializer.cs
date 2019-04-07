using Axiverse.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Assets.M3
{
    public class M3Deserializer : IResourceLoader<object>
    {
        public string Name => "Blizzard m3 model loader";

        public string[] Extensions => new string[]{".m3"};

        public bool TryLoad(Stream stream, out object value)
        {

            value = null;
            return false;
        }

        struct ReferenceEntry
        {
            uint type;
            uint offset;
            uint count;
            uint version;
        }

        struct Reference
        {
            uint entries;
            uint id;
        }

        struct Md33
        {
            uint token; // 'MD33'
            uint offsetToTable;
            uint entriesOfTable;
            Reference model;
        }
    }
}
