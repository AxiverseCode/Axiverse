using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Windows
{
    public partial class Control
    {
        private Rectangle m_bounds;

        /// <summary>
        /// 
        /// </summary>
		public float Left
        {
            get => m_bounds.Left;
            set => Location = new Vector2(value, Top);
        }

        /// <summary>
        /// 
        /// </summary>
        public float Right
        {
            get => m_bounds.Right;
            set => Location = new Vector2(value - Width, Top);
        }

        /// <summary>
        /// 
        /// </summary>
        public float Top
        {
            get => m_bounds.Top;
            set => Location = new Vector2(Left, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public float Bottom
        {
            get => m_bounds.Bottom;
            set => Location = new Vector2(Left, value - Height);
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Location
        {
            get => m_bounds.Location;
            set
            {
                m_bounds.Location = value;
                OnLocationChanged(this, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Width
        {
            get => m_bounds.Width;
            set => Size = new Vector2(value, Height);
        }

        /// <summary>
        /// 
        /// </summary>
        public float Height
        {
            get => m_bounds.Height;
            set => Size = new Vector2(Width, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Size
        {
            get => m_bounds.Size;
            set
            {
                m_bounds.Size = value;
                OnSizeChanged(this, null);
            }
        }

        public Rectangle Bounds
        {
            get { return m_bounds; }
            set
            {
                bool locationChanged = value.Location != m_bounds.Location;
                bool sizeChanged = value.Size != m_bounds.Size;
                m_bounds = value;

                if (locationChanged)
                {
                    OnLocationChanged(this, null);
                }

                if (sizeChanged)
                {
                    OnSizeChanged(this, null);
                }
            }
        }

        public void Translate(float x, float y)
        {
            Location += new Vector2(x, y);
        }

        public void Translate(Vector2 offset)
        {
            Location += offset;
        }

        public Control FindControl(Vector2 point)
        {
            if (!Contains(point))
            {
                return null;
            }

            var localPoint = point - Location;
            Control contained;

            foreach (var child in Children)
            {
                contained = child.FindControl(localPoint);
                if (contained != null)
                {
                    //System.Diagnostics.Debug.WriteLine(contained.Name);
                    return contained;
                }
            }

            return this;
        }

        public bool Contains(Vector2 point)
        {
            return Bounds.Contains(point);
        }

        public event EventHandler LocationChanged;
        public event EventHandler SizeChanged;

        public virtual void OnLocationChanged(object sender, EventArgs e)
        {
            LocationChanged?.Invoke(this, e);
        }

        public virtual void OnSizeChanged(object sender, EventArgs e)
        {
            SizeChanged?.Invoke(this, e);
        }
    }
}
