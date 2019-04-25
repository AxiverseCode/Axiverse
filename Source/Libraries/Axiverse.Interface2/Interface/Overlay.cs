using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace Axiverse.Interface2.Interface
{
    using Color = Axiverse.Mathematics.Drawing.Color;

    public class Overlay : Control
    {
        public override void Draw(Canvas canvas)
        {
            // This is an overlay, draw it last but in order.
            Chrome.Overlay(this, canvas.NativeDeviceContext.Transform);
        }

        public void DrawOverlay(Canvas canvas)
        {
            base.Draw(canvas);
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
