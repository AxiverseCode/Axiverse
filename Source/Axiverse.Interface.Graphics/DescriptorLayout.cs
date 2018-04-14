using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics
{
    public class DescriptorLayout
    {
        public int ShaderResourceViewCount;
        public int SamplerCount;

        public Entry[] Entries { get; }

        public DescriptorLayout(params EntryType[] types)
        {
            var entries = new Entry[types.Length];
            foreach (var type in types)
            {
                var entry = new Entry()
                {
                    Type = type,
                    Index = (type == EntryType.ShaderResourceView) ? ShaderResourceViewCount++ : SamplerCount++
                };
            }

            Entries = entries;
        }


        public class Entry
        {
            public EntryType Type;
            public int Index;
        }

        public enum EntryType
        {
            ShaderResourceView,
            SamplerState,
        }
    }
}
