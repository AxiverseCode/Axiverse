using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Windows
{
    public class Font
    {
        public string FamilyName { get; }

        public float Size { get; }

        public FontWeight Weight { get; }

        public Font(string familyName, float size, FontWeight weight)
        {
            FamilyName = familyName;
            Size = size;
            Weight = weight;
        }

        public override bool Equals(object obj)
        {
            if (obj is Font font)
            {
                return (font.FamilyName == FamilyName) &&
                    (font.Size == Size) &&
                    (font.Weight == Weight);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return FamilyName.GetHashCode() ^ Size.GetHashCode() ^ Weight.GetHashCode();
        }
    }
}
