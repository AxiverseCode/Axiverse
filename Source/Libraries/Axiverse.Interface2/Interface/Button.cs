using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace Axiverse.Interface2.Interface
{
    public class Button : Control
    {
        public Button()
        {
            Backcolor = new Color(0, 1f, 0);
        }

        protected override void OnDraw(DeviceContext context)
        {
            using(var brush = new SolidColorBrush(context, new Color(0, 0, 0)))
            {
                if (Backcolor != Color.Transparent)
                {
                    context.Clear(Backcolor);
                }
                context.DrawRectangle(new RectangleF(2.5f, 2.5f, 8, 8), brush);
            }
        }

        protected internal override void OnMouseEnter(MouseEventArgs e)
        {
            Backcolor = new Color(0, 1f, 1f);
        }

        protected internal override void OnMouseLeave(MouseEventArgs e)
        {
            Backcolor = new Color(0, 1f, 0);
        }
    }
}
