using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Direct2D1;

using NativeMouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace Axiverse.Interface2.Interface
{
    public class Chrome
    {
        public ControlCollection Controls { get; }

        public Chrome(Form form)
        {
            Controls = new ControlCollection(this);

            form.MouseMove += HandleMouseMove;
            form.MouseDown += HandleMouseDown;
            form.MouseUp += HandleMouseUp;
            form.MouseWheel += HandleMouseWheel;
            
        }

        public void Draw(DeviceContext context)
        {
            var transform = context.Transform;
            foreach (var control in Controls)
            {
                control.Draw(context);
                // Extract all overlays.
            }
            context.Transform = transform;

            // Draw all overlays.
        }

        private Control FindControl(Vector2 point)
        {
            Control control;
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                control = Controls[i].FindControl(point);
                if (control != null)
                {
                    return control;
                }
            }
            return null;
        }

        private Control selected;
        private Control hover;
        private Control click;
        private Vector2 position;

        private void HandleMouseWheel(object sender, NativeMouseEventArgs e)
        {

        }

        private void HandleMouseUp(object sender, NativeMouseEventArgs e)
        {
            var point = new Vector2(e.X, e.Y);
            var args = new MouseEventArgs(point, Vector2.Zero);

            click?.OnMouseUp(args);
            click = null;

            var newHover = FindControl(point);

            if (newHover != hover)
            {
                hover?.OnMouseLeave(null);
                newHover?.OnMouseEnter(null);
                hover = newHover;
            }
        }

        public void HandleMouseMove(object sender, NativeMouseEventArgs e)
        {
            var point = new Vector2(e.X, e.Y);
            var movement = point - position;
            position = point;

            var args = new MouseEventArgs(point, movement);

            if (click == null)
            {
                hover?.OnMouseMove(args);

                var newHover = FindControl(point);

                if (hover != newHover)
                {
                    hover?.OnMouseLeave(args);
                    newHover?.OnMouseEnter(args);
                    hover = newHover;
                }
            }
            else
            {
                click.OnMouseMove(args);
            }
        }

        public void HandleMouseDown(object sender, NativeMouseEventArgs e)
        {
            var point = new Vector2(e.X, e.Y);
            var args = new MouseEventArgs(point, Vector2.Zero);

            hover?.OnMouseDown(args);

            if (hover != null)
            {
                click = hover;
            }
        }
    }
}
