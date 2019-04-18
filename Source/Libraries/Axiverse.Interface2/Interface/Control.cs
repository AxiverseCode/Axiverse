using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace Axiverse.Interface2.Interface
{
    public class Control
    {
        private Chrome overlay;
        public Chrome Overlay {
            get => overlay;
            internal set
            {
                if (value != overlay)
                {
                    overlay = value;
                    foreach (var child in Children)
                    {
                        child.Overlay = overlay;
                    }
                }
            }
        }


        public Control Parent { get; internal set; }
        public ControlCollection Children { get; }

        public string Text { get; set; }

        public Control()
        {
            Children = new ControlCollection(this);
            Position = new Vector2(10, 10);
            Size = new Vector2(50, 50);
        }

        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public Color Forecolor { get; set; } = new Color(0, 0, 0);
        public Color Backcolor { get; set; } = Color.Transparent;

        public Control FindControl(Vector2 point)
        {
            if (!Contains(point))
            {
                return null;
            }

            var relative = point - Position;
            Control control;

            // Search backwards, the foreground control gets precidence.
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                control = Children[i].FindControl(relative);
                if (control != null)
                {
                    return control;
                }
            }

            return this;
        }

        /// <summary>
        /// Calculates whether the control contains a point in the context of its parent.
        /// TODO: Determine whether this is preferred or in global context.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Vector2 point)
        {
            return
                (point.X > Position.X && point.X < Position.X + Size.X) &&
                (point.Y > Position.Y && point.Y < Position.Y + Size.Y);
        }

        public virtual void Draw(DeviceContext context)
        {
            var transform = context.Transform;
            context.Transform = Matrix3x2.Multiply(context.Transform, Matrix3x2.Translation(Position.X, Position.Y));
            context.PushAxisAlignedClip(new RectangleF(0, 0, Size.X, Size.Y), AntialiasMode.Aliased);

            // Draw this control.
            OnDraw(context);

            // Recursively draw every child control.
            foreach (var child in Children)
            {
                child.Draw(context);
            }

            context.PopAxisAlignedClip();
            context.Transform = transform;
        }

        protected virtual void OnDraw(DeviceContext context)
        {
            if (Backcolor != Color.Transparent)
            {
                if (Backcolor.A != 255)
                {
                    using (var brush = new SolidColorBrush(context, Backcolor))
                    {
                        context.FillRectangle(new RectangleF(0, 0, Size.X, Size.Y), brush);
                    }
                }
                else
                {
                    context.Clear(Backcolor);
                }
            }
         }

        protected internal virtual void OnMouseDown(MouseEventArgs e) { }

        protected internal virtual void OnMouseUp(MouseEventArgs e) { }

        protected internal virtual void OnMouseMove(MouseEventArgs e) { }

        protected internal virtual void OnMouseEnter(MouseEventArgs e) { }

        protected internal virtual void OnMouseLeave(MouseEventArgs e) { }
    }
}
