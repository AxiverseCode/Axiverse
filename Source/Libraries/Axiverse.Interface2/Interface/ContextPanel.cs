using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Animations;

namespace Axiverse.Interface2.Interface
{
    public class ContextPanel : Overlay
    {
        private object delayContext = new object();

        public float Delay { get; set; } = 0.5f;
        public bool Captured { get; private set; }
        private bool hovered = false;

        public virtual void Capture()
        {
            Visible = true;
            Captured = true;
            Chrome.Animator.RemoveExclusive(delayContext);
        }

        public void Release()
        {
            Captured = false;
            if (!hovered)
            {
                Chrome.Animator.AddExclusive(delayContext, new DelayAnimation(Delay, (s, e) => Visible = false));
            }
        }

        protected internal override void OnMouseEnter(MouseEventArgs e)
        {
            hovered = true;
            Chrome.Animator.RemoveExclusive(delayContext);
        }

        protected internal override void OnMouseLeave(MouseEventArgs e)
        {
            hovered = false;
            if (Visible)
            {
                Chrome.Animator.AddExclusive(delayContext, new DelayAnimation(Delay, (s, args) => Visible = false));
            }
        }
    }
}
