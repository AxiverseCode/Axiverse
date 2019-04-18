using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Axiverse.Interface2
{
    public static class ConversionExtensions
    {
        public static RectangleF ToRectangleF(this Rectangle r)
        {
            return new RectangleF(r.X, r.Y, r.Width, r.Height);
        }
    }
}
