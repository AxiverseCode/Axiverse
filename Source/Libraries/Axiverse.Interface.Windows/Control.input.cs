using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Windows
{

    public partial class Control
    {
        public int? m_tabIndex;
        public virtual int? TabIndex
        {
            get => m_tabIndex;
            set
            {
                if (value != m_tabIndex)
                {
                    m_tabIndex = value;
                    OnTabIndexChanged(this, null);
                }
            }
        }

        public event EventHandler TabIndexChanged;

        protected virtual void OnTabIndexChanged(object sender, EventArgs e)
        {
            TabIndexChanged?.Invoke(sender, e);
        }


        public event EventHandler<MouseEventArgs> MouseDown;
        public event EventHandler<MouseEventArgs> MouseUp;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;
        public event EventHandler<MouseMoveEventArgs> MouseMove;
        public event EventHandler<MouseMoveEventArgs> MouseWheel;

        protected internal virtual void OnMouseDown(object sender, MouseEventArgs e)
        {
            MouseDown?.Invoke(sender, e);
        }

        protected internal virtual void OnMouseUp(object sender, MouseEventArgs e)
        {
            MouseUp?.Invoke(sender, e);
        }

        protected internal virtual void OnMouseEnter(object sender, EventArgs e)
        {
            MouseEnter?.Invoke(sender, e);
        }

        protected internal virtual void OnMouseLeave(object sender, EventArgs e)
        {
            MouseLeave?.Invoke(sender, e);
        }

        protected internal virtual void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            MouseMove?.Invoke(sender, e);
        }

        protected internal virtual void OnMouseWheel(object sender, MouseMoveEventArgs e)
        {
            MouseWheel?.Invoke(sender, e);
        }
    }
}