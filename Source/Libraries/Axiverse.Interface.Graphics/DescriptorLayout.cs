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

        public List<Entry> Entries { get; } = new List<Entry>();

        public DescriptorLayout(params EntryType[] types)
        {
            var entries = new Entry[types.Length];
            foreach (var type in types)
            {
                Entries.Add(new Entry()
                {
                    Slot = Entries.Count,
                    Type = type,
                    Index = (type == EntryType.ConstantBufferShaderResourceOrUnorderedAccessView) ? ShaderResourceViewCount++ : SamplerCount++
                });
            }
        }


        public class Entry
        {
            public EntryType Type;
            public int Index;
            public int Slot;
        }

        public enum EntryType
        {
            ConstantBufferShaderResourceOrUnorderedAccessView,
            SamplerState,
        }
    }
}
