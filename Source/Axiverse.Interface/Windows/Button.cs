using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Windows
{
    public class Button : Control
    {
        public bool Hover { get; private set; }

        public bool Active { get; private set; }

        public Color HoverColor { get; private set; }

        public Color ActiveColor { get; private set; }

        public Button()
        {
            BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 1);
            ForegroundColor = new Color(1, 1, 1, 1);
            HoverColor = DefaultHoverColor;
            ActiveColor = DefaultActiveColor;
            Size = new Vector2(100, 40);
        }

        protected internal override void OnMouseEnter(object sender, EventArgs e)
        {
            Hover = true;
            base.OnMouseEnter(sender, e);
        }

        protected internal override void OnMouseLeave(object sender, EventArgs e)
        {
            Hover = false;
            base.OnMouseLeave(sender, e);
        }

        protected internal override void OnMouseDown(object sender, MouseEventArgs e)
        {
            Active = true;
            base.OnMouseDown(sender, e);
        }

        protected internal override void OnMouseUp(object sender, MouseEventArgs e)
        {
            Active = false;
            base.OnMouseUp(sender, e);
        }

        public override void Draw(Compositor compositor)
        {
            var color = Active ? ActiveColor : Hover ? HoverColor : BackgroundColor;
            var layout = new TextLayout()
            {
                VerticalAlignment = VerticalAlign.Middle,
                HorizontalAlignment = HorizontalAlign.Center,
            };

            var rectangle = new Rectangle(0, 0, Width, Height);
            var rectangle2 = new Rectangle(0.5f, 0.5f, Width - 1, Height - 1);
            compositor.FillRoundedRectangle(rectangle, new Vector2(3), color);
            compositor.DrawRoundedRectangle(rectangle2, new Vector2(2), new Color(1, 1, 1, 0.2f));
            compositor.DrawText(Text, Font, layout, rectangle, ForegroundColor);
        }

        public static Color DefaultHoverColor => new Color(0.2f, 0.2f, 0.2f, 1);

        public static Color DefaultActiveColor => new Color(0.15f, 0.15f, 0.15f, 1);
    }
}
