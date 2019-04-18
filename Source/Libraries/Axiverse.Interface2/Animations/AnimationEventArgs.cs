using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Animations
{
    public class AnimationEventArgs : EventArgs
    {
        public float Age { get; set; }

        public bool Alive { get; private set; } = true;

        public void Remove()
        {
            Alive = false;
        }
    }

    public delegate void AnimationEventHandler(object sender, AnimationEventArgs e);
}
