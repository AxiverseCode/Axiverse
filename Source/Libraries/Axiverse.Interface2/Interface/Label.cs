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

        protected override void OnDraw(DeviceContext context)
        {
            using (var factory = new Factory())
            using (var brush = new SolidColorBrush(context, Forecolor))
            using (var format = new TextFormat(factory, "Calibri", 10))
            {
                if (Backcolor != Color.Transparent)
                {
                    context.Clear(Backcolor);
                }

                format.ParagraphAlignment = ParagraphAlignment.Center;
                format.TextAlignment = TextAlignment.Center;
                context.DrawText(Text, format, new RectangleF(0, 0, Size.X, Size.Y), brush);
            }
        }
    }
}
