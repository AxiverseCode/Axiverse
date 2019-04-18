using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX;
using SharpDX.DirectWrite;
using Factory = SharpDX.DirectWrite.Factory;

namespace Axiverse.Interface2.Interface
{
    public class Slider : Control
    {
        public Color Color { get; set; } = new Color(0, 85, 221);
        public float Minimum { get; set; }
        public float Maximum { get; set; } = 100;
        public float Value { get; set; } = 50;

        protected override void OnDraw(DeviceContext context)
        {
            base.OnDraw(context);

            const float border = 2;

            using (var factory = new Factory())
            using (var format = new TextFormat(factory, "Calibri", 18))
            using (var brush = new SolidColorBrush(context, Color))
            {
                var rect = new RectangleF(border, border, Size.X - border * 2, Size.Y - border * 2);
                float percentage = (Value - Minimum) / (Maximum - Minimum);
                float width = percentage * (Size.X - border * 2);
                context.FillRectangle(new RectangleF(rect.X, rect.Y, rect.Width * percentage, rect.Height), brush);

                rect.X += border;
                rect.Width -= border * 2;

                format.ParagraphAlignment = ParagraphAlignment.Center;
                brush.Color = Forecolor;

                format.TextAlignment = TextAlignment.Leading;
                context.DrawText(Text, format, rect, brush);

                format.TextAlignment = TextAlignment.Trailing;
                context.DrawText(Value.ToString(), format, rect, brush);
            }
        }

        bool variate = false;

        protected internal override void OnMouseDown(MouseEventArgs e)
        {
            variate = true;
        }

        protected internal override void OnMouseUp(MouseEventArgs e)
        {
            variate = false;
        }

        protected internal override void OnMouseMove(MouseEventArgs e)
        {
            if (variate)
            {
                var value = Value + e.Movement.X;
                Value = Functions.Clamp(value, Minimum, Maximum);
            }
        }

        protected internal override void OnMouseEnter(MouseEventArgs e)
        {
            Backcolor = new Color(0.2f, 0.2f, 0.2f, 1f);
        }

        protected internal override void OnMouseLeave(MouseEventArgs e)
        {
            Backcolor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        }
    }
}
