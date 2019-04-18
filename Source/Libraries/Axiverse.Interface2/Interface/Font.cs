using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Interface
{
    public class Font
    {
        /// <summary>
        /// Gets the font family of the font.
        /// </summary>
        public string FontFamily { get; }
        
        /// <summary>
        /// Gets the font size of the font.
        /// </summary>
        public float Size { get; }

        public Font(string fontFamily, float size)
        {
            FontFamily = fontFamily;
            Size = size;
        }
    }
}
