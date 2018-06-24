using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct2D1;

namespace Axiverse.Interface.Windows
{
    public partial class Control
    {
        #region BackgroundColor

        private Color m_backgroundColor;

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public Color BackgroundColor
        {
            get => m_backgroundColor;
            set
            {
                if (value != m_backgroundColor)
                {
                    m_backgroundColor = value;
                    OnBackgroundColorChanged(this, null);
                }
            }
        }

        public event EventHandler BackgroundColorChanged;

        protected virtual void OnBackgroundColorChanged(object sender, EventArgs e)
        {
            BackgroundColorChanged?.Invoke(sender, e);
        }

        #endregion

        public Color ForegroundColor { get; set; }

        protected virtual void OnDraw()
        {

        }

        protected virtual void OnAnimate()
        {

        }

        public void Invalidate()
        {

        }

        public void Invalidate(RectangleF rectangle)
        {

        }

        public virtual void Draw(DrawContext compositor)
        {
            compositor.FillRoundedRectangle(new Rectangle(0, 0, Width, Height), new Vector2(0, 0), BackgroundColor);
            compositor.DrawText(Text, Font, new TextLayout(), new Rectangle(0, 0, Width, Height), ForegroundColor);
        }

        public virtual void DrawChildren(DrawContext compositor)
        {
            DrawChildren(Vector2.Zero, compositor);
        }

        public virtual void DrawChildren(Vector2 parentOffset, DrawContext context)
        {
            // could be optimized as transforms affects axis aligned clips
            var offset = parentOffset + Bounds.TopLeft;
            Matrix3x2 transform = Matrix3x2.Translation(offset.ToVector2());
            RectangleF absoluteBounds = new RectangleF(offset.X, offset.Y, Bounds.Width, Bounds.Height);

            context.DeviceContext.PushAxisAlignedClip(absoluteBounds, AntialiasMode.Aliased);

            context.DeviceContext.Transform = transform;
            Draw(context);

            context.DeviceContext.Transform = Matrix3x2.Identity;
            foreach (var child in Children)
            {
                child.DrawChildren(offset, context);
            }

            context.DeviceContext.PopAxisAlignedClip();
        }
    }
}
