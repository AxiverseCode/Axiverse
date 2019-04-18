using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Animations
{
    public class IntervalAnimation : IAnimation
    {
        private AnimationEventArgs args = new AnimationEventArgs();
        private float remaining;

        public float Interval { get; }

        public AnimationEventHandler Callback { get; set; }

        public bool Advance(object context, float delta)
        {
            args.Age += delta;
            remaining -= delta;

            while (remaining < 0)
            {
                remaining += Interval;
                Callback(context, args);
                return args.Alive;
            }

            return true;
        }

        public IntervalAnimation(float interval, AnimationEventHandler callback)
        {
            Interval = interval;
            Callback = callback;

            remaining = interval;
        }
    }
}
