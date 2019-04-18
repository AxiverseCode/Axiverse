using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Factory = SharpDX.DirectWrite.Factory;

namespace Axiverse.Interface2.Interface
{
    public class Label : Control
    {
        public Label()
        {
            Size = new Vector2(80, 20);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            var context = canvas.NativeDeviceContext;
            var brush = canvas.GetBrush(Forecolor);
            var format = canvas.GetTextFormat(Font);

            format.ParagraphAlignment = ParagraphAlignment.Center;
            format.TextAlignment = TextAlignment.Center;
            context.DrawText(Text, format, new RectangleF(0, 0, Size.X, Size.Y), brush);
        }
    }
}
