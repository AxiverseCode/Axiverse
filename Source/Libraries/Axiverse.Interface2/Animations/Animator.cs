using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Animations
{
    public class Animator
    {
        private readonly List<IAnimation> animations = new List<IAnimation>();
        private readonly Dictionary<object, IAnimation> exclusiveAnimations = new Dictionary<object, IAnimation>();
        private readonly List<object> exclusiveRemovals = new List<object>();

        public void Advance(float timeChange)
        {
            for (int i = 0; i < animations.Count; i++)
            {
                if (!animations[i].Advance(null, timeChange))
                {
                    animations.RemoveAt(i--);
                }
            }

            foreach (var pair in exclusiveAnimations)
            {
                if (!pair.Value.Advance(pair.Key, timeChange))
                {
                    exclusiveRemovals.Add(pair.Key);
                }
            }

            foreach (var item in exclusiveRemovals)
            {
                exclusiveAnimations.Remove(item);
            }
            exclusiveRemovals.Clear();
        }

        public void Add(IAnimation animation)
        {
            if (!animations.Contains(animation))
            {
                animations.Add(animation);
            }
        }

        public void AddExclusive(object context, IAnimation animation)
        {
            exclusiveAnimations[context] = (animation);
        }

        public void RemoveExclusive(object context)
        {
            exclusiveAnimations.Remove(context);
        }
    }
}
