using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Animations
{
    public class DelayAnimation : IAnimation
    {
        private AnimationEventArgs args = new AnimationEventArgs();
        public float Delay { get; }

        public AnimationEventHandler Callback { get; set; }

        public bool Advance(object context, float delta)
        {
            args.Age += delta;

            if (args.Age > Delay)
            {
                args.Remove();
                Callback(context, args);
                return false;
            }

            return true;
        }

        public DelayAnimation(float delay, AnimationEventHandler callback)
        {
            Delay = delay;
            Callback = callback;
        }
    }
}
