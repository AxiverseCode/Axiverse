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

            using (var factory = new Factory())
            using (var brush = new SolidColorBrush(context, Forecolor))
            using (var format = new TextFormat(factory, "Calibri", 20))
            {
                format.ParagraphAlignment = ParagraphAlignment.Center;
                format.TextAlignment = TextAlignment.Leading;

                for (int i = 0; i < Items.Count; i++)
                {
                    var rect = new RectangleF(Margin, Margin + 40 * i, Size.X - Margin * 2, 40);
                    if (selectedItem == i)
                    {
                        brush.Color = new Color(0, 85, 221);
                        context.FillRectangle(rect, brush);
                        brush.Color = Forecolor;
                    }
                    rect.X += 10; rect.Width -= 20;
                    context.DrawText(Items[i].Text ?? "", format, rect, brush);
                }
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
