using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Animations
{
    public class ContinuousAnimation : IAnimation
    {
        private readonly AnimationEventArgs args = new AnimationEventArgs();

        public AnimationEventHandler Callback { get; set; }

        public bool Advance(object context, float delta)
        {
            args.Age += delta;
            Callback(context, args);

            return args.Alive;
        }

        public ContinuousAnimation(AnimationEventHandler callback)
        {
            Callback = callback;
        }
    }
}
