using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Windows
{
    public partial class Control
    {
        private string m_name;

        public virtual string Name
        {
            get => m_name;
            set
            {
                if (value != m_name)
                {
                    m_name = value;
                    OnNameChanged(this, null);
                }
            }
        }

        public event EventHandler NameChanged;

        protected virtual void OnNameChanged(object sender, EventArgs e)
        {
            NameChanged?.Invoke(sender, e);
        }

        private string m_text;

        public virtual string Text
        {
            get => m_text;
            set
            {
                if (value != m_text)
                {
                    m_text = value;
                    OnTextChanged(this, null);
                }
            }
        }

        public event EventHandler TextChanged;

        protected virtual void OnTextChanged(object sender, EventArgs e)
        {
            TextChanged?.Invoke(sender, e);
        }

        private object m_tag;

        public virtual object Tag
        {
            get => m_tag;
            set
            {
                if (value != m_tag)
                {
                    m_tag = value;
                    OnTagChanged(this, null);
                }
            }
        }

        public event EventHandler TagChanged;

        protected virtual void OnTagChanged(object sender, EventArgs e)
        {
            TagChanged?.Invoke(sender, e);
        }

        private bool m_enabled;

        public virtual bool Enabled
        {
            get => m_enabled;
            set
            {
                if (value != m_enabled)
                {
                    m_enabled = value;
                    OnEnabledChanged(this, null);
                }
            }
        }

        public event EventHandler EnabledChanged;

        protected virtual void OnEnabledChanged(object sender, EventArgs e)
        {
            EnabledChanged?.Invoke(sender, e);
        }

        private bool m_animated;

        public virtual bool Animated
        {
            get => m_animated;
            set
            {
                if (value != m_animated)
                {
                    m_animated = value;
                    OnAnimatedChanged(this, null);
                }
            }
        }

        public event EventHandler AnimatedChanged;

        protected virtual void OnAnimatedChanged(object sender, EventArgs e)
        {
            AnimatedChanged?.Invoke(sender, e);
        }
    }
}
