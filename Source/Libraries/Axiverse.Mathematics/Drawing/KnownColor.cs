using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Drawing
{
    public class KnownColor
    {
        private Color color;

        /// <summary>
        /// Gets the name of the <see cref="KnownColor"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets if the <see cref="KnownColor"/> is a static value.
        /// </summary>
        public bool IsStatic { get; }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> value of the <see cref="KnownColor"/>.
        /// </summary>
        public Color Color {
            get => color;
            set
            {
                if (IsStatic)
                {
                    throw new InvalidOperationException("Static color.");
                }
                color = value.ToAuthoritative();
            }
        }

        private KnownColor(string name, bool isStatic, Color initialColor)
        {
            Name = name;
            IsStatic = isStatic;
            color = initialColor.ToAuthoritative();
        }

        public static KnownColor OfConstant(string name, Color initialColor)
        {
            return new KnownColor(name, true, initialColor);
        }

        public static KnownColor OfName(string name)
        {
            return new KnownColor(name, false, Color.Magenta);
        }

        public static KnownColor OfName(string name, Color initialColor)
        {
            return new KnownColor(name, false, initialColor);
        }
    }
}
