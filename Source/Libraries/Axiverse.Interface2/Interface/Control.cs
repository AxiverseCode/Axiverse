using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using Axiverse.Mathematics.Drawing;

namespace Axiverse.Interface2.Interface
{
    public class Control
    {
        #region Properties

        #region Properties - Hierarchy

        private Chrome chrome;
        /// <summary>
        /// Gets the <see cref="Chrome"/> which the control is attached to.
        /// </summary>
        public Chrome Chrome {
            get => chrome;
            internal set
            {
                if (value != chrome)
                {
                    chrome?.OnControlRemoved(this);
                    chrome = value;
                    value?.OnControlAdded(this);
                    foreach (var child in Children)
                    {
                        child.Chrome = chrome;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the parent control of the control.
        /// </summary>
        public Control Parent { get; internal set; }

        /// <summary>
        /// Gets the children of the control.
        /// </summary>
        public ControlCollection Children { get; }

        #endregion

        #region Properties - Spatial

        private Rectangle bounds;

        /// <summary>
        /// Gets or sets the bounds of the control.
        /// </summary>
        public Rectangle Bounds
        {
            get => bounds;
            set => bounds = value;
        }

        /// <summary>
        /// Gets or sets the position of the control.
        /// </summary>
        public Vector2 Position
        {
            get => bounds.Location;
            set => bounds.Location = value;
        }

        /// <summary>
        /// Gets or sets the size of the control.
        /// </summary>
        public Vector2 Size
        {
            get => bounds.Size;
            set => bounds.Size = value;
        }

        /// <summary>
        /// Gets or sets the X of the control.
        /// </summary>
        public float X
        {
            get => bounds.X;
            set => bounds.X = value;
        }

        /// <summary>
        /// Gets or sets the Y of the control.
        /// </summary>
        public float Y
        {
            get => bounds.Y;
            set => bounds.Y = value;
        }

        /// <summary>
        /// Gets or sets the width of the control.
        /// </summary>
        public float Width
        {
            get => bounds.Width;
            set => bounds.Width = value;
        }

        /// <summary>
        /// Gets or set height of the control.
        /// </summary>
        public float Height
        {
            get => bounds.Height;
            set => bounds.Height = value;
        }

        #endregion

        #region Properties - Visual

        /// <summary>
        /// Gets or sets the font of the control.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Gets or sets the text of the control.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the forecolor of the control.
        /// </summary>
        public Color Forecolor { get; set; } = new Color(0, 0, 0);

        /// <summary>
        /// Gets or sets the backcolor of the control.
        /// </summary>
        public Color Backcolor { get; set; } = Color.Transparent;

        /// <summary>
        /// Gets or sets if the control is visible.
        /// </summary>
        public bool Visible { get; set; } = true;

        #endregion

        #endregion

        public Control()
        {
            Children = new ControlCollection(this);

            Size = new Vector2(80, 24);
            Font = new Font("Calibri", 20);
        }

        /// <summary>
        /// Calculates whether the control contains a point in the context of the control.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Vector2 point)
        {
            return
                (point.X > 0 && point.X < Size.X) &&
                (point.Y > 0 && point.Y < Size.Y);
        }

        public Control FindControl(Vector2 point)
        {
            if (!Visible || !Contains(point))
            {
                return null;
            }
            Control control;

            // Search backwards, the foreground control gets precidence.
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                var relative = point - Children[i].Position;
                control = Children[i].FindControl(relative);
                if (control != null)
                {
                    return control;
                }
            }

            return this;
        }
        public Vector2 FindLocalPoint(Vector2 globalPoint)
        {
            var offset = new Vector2();
            for (Control c = this; c != null; c = c.Parent)
            {
                offset += c.Position;
            }
            return globalPoint - offset;
        }

        public virtual void Draw(Canvas canvas)
        {
            var context = canvas.NativeDeviceContext;

            if (!Visible)
            {
                return;
            }

            var transform = context.Transform;
            context.Transform = Matrix3x2.Multiply(context.Transform, Matrix3x2.Translation(Position.X, Position.Y));
            context.PushAxisAlignedClip(new RectangleF(0, 0, Size.X, Size.Y), AntialiasMode.Aliased);

            // Draw this control.
            OnDraw(canvas);

            // Recursively draw every child control.
            foreach (var child in Children)
            {
                child.Draw(canvas);
            }

            context.PopAxisAlignedClip();
            context.Transform = transform;
        }

        #region Handlers

        protected virtual void OnDraw(Canvas canvas)
        {
            var context = canvas.NativeDeviceContext;

            if (Backcolor != Color.Transparent)
            {
                if (Backcolor.A != 255)
                {
                    var brush = canvas.GetBrush(Backcolor);
                    context.FillRectangle(new RectangleF(0, 0, Size.X, Size.Y), brush);
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

        #endregion
    }
}
