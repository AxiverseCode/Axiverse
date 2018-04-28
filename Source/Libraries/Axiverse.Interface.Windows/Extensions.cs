using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace Axiverse.Interface.Windows
{
    public static class Extensions
    {
        public static RectangleF ToRectangleF(this Rectangle rectangle)
        {
            return new RectangleF(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
        }

        public static Color4 ToColor4(this Color color) => new Color4(color.Red, color.Green, color.Blue, color.Opacity);

        public static SharpDX.Vector2 ToVector2(this Vector2 vector) => new SharpDX.Vector2(vector.X, vector.Y);
    }
}
