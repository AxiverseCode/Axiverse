using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public class AutoValue : IDynamic
    {
        /// <summary>
        /// The current value.
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// The maximum value.
        /// </summary>
        public float Maximum { get; set; }

        /// <summary>
        /// The minimum value.
        /// </summary>
        public float Minimum { get; set; }

        /// <summary>
        /// The amount of regeneration per second.
        /// </summary>
        public float Regeneration { get; set; }

        /// <summary>
        /// Step the value.
        /// </summary>
        /// <param name="delta"></param>
        public virtual void Step(float delta)
        {
            if (Value < Maximum)
            {
                Value = Math.Min(Value + Regeneration * delta, Maximum);
            }
        }
    }
}
