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
    using Color = Axiverse.Mathematics.Drawing.Color;

    public class ContextMenu : ContextPanel
    {
        const float Margin = 5;
        private int selectedItem = -1;

        public List<MenuItem> Items { get; } = new List<MenuItem>();


        public override void Capture()
        {
            base.Capture();
            Size = new Vector2(200, 40 * Items.Count + Margin * 2);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            var context = canvas.NativeDeviceContext;

            var brush = canvas.GetBrush(Forecolor);
            var format = canvas.GetTextFormat(Font);
            format.ParagraphAlignment = ParagraphAlignment.Center;
            format.TextAlignment = TextAlignment.Leading;

            for (int i = 0; i < Items.Count; i++)
            {
                var rect = new RectangleF(Margin, Margin + 40 * i, Size.X - Margin * 2, 40);
                if (selectedItem == i)
                {
                    brush.Color = InterfaceColors.ControlHover.ToRawColor4();
                    context.FillRectangle(rect, brush);
                    brush.Color = Forecolor.ToRawColor4();
                }
                rect.X += 10; rect.Width -= 20;
                context.DrawText(Items[i].Text ?? "", format, rect, brush);
            }
        }

        protected internal override void OnMouseMove(MouseEventArgs e)
        {
            selectedItem = (int)((e.Position.Y - Margin) / 40);
        }

        protected internal override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            selectedItem = -1;
        }
    }
}
