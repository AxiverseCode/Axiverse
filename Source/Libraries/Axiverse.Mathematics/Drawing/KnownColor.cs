using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Drawing
{
    public class KnownColor
    {
        public string Name { get; }
        public bool IsStatic { get; }

        public Color Color { get; set; }

        private KnownColor(string name, bool isStatic, Color value)
        {
            Name = name;
            IsStatic = isStatic;
            Color = value;
        }

        public static KnownColor CreateStatic(string name, Color value)
        {
            if (value.Template != null)
            {
                throw new ArgumentException();
            }
            return new KnownColor(name, true, value);
        }
    }
}
