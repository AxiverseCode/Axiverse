using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Axiverse.Interface2.Interface.Patterns
{
    public class Scroller
    {
        public float Length;
        public float Range;
        public float Offset;
        public float TargetOffset;
        public float MaxOffset;

        public bool IsScrollable => Length < Range;

        private Control Control;

        public Scroller(Control control)
        {
            Control = control;
        }

        public void Draw(Vector2 size, Canvas canvas)
        {
            if (IsScrollable)
            {
                const int margins = 4;
                const int width = 10;


                var rect = new RectangleF(size.X - margins - width, margins, width, size.Y - margins * 2);

                float saturation = Length / Range;
                float height = Math.Max(width, saturation * (rect.Height - 10) + 10);
                float offset = Offset / MaxOffset * (rect.Height - height);

                rect.Y += offset;
                rect.Height = height;

                var brush = canvas.GetBrush(Control.Forecolor);
                canvas.NativeDeviceContext.FillRectangle(rect, brush);
            }
        }

        public void Update()
        {
            MaxOffset = Range - Length;
            Offset = Functions.Clamp(Offset, 0, MaxOffset);


        }

        public void Scroll(float z)
        {
            Offset = Functions.Clamp(Offset + z, 0, MaxOffset);
        }

        public Vector2 ToScrollSpace(Vector2 screenSpace)
        {
            return new Vector2(screenSpace.X, screenSpace.Y + Offset);
        }

        public Vector2 ToScreenSpace(Vector2 scrollSpace)
        {
            return new Vector2(scrollSpace.X, scrollSpace.Y - Offset);
        }
    }
}
