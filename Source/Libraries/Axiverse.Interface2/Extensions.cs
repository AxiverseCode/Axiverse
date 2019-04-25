using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Mathematics.Interop;
using Axiverse.Mathematics.Drawing;

namespace Axiverse.Interface2
{
    public static class Extensions
    {
        public static RawColor4 ToRawColor4(this Color value)
        {
            return new RawColor4(value.R, value.G, value.B, value.A);
        }
    }
}
