using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;

namespace Axiverse.Simulation
{
    public class ThrustEquiptment : Equiptment
    {
        /// <summary>
        /// The maximum amount of thrust the engine in Newtons.
        /// </summary>
        public float MaximumThrust { get; set; }

        /// <summary>
        /// The amount of thrust we want to achieve in Newtons.
        /// </summary>
        public float TargetThrust { get; set; }

        /// <summary>
        /// The amount thrust can change per second.
        /// </summary>
        public float ThrustAcceleration { get; private set; }

        /// <summary>
        /// The current amount of thrust in Newtons.
        /// </summary>
        public float Thrust { get; private set; }

        /// <summary>
        /// The direction that the vehicle wants to travel
        /// </summary>
        public Vector3 Direction { get; set; }

        public void Step(Entity entity, double delta)
        {

            // change thrust
            TargetThrust = Math.Min(MaximumThrust, TargetThrust);
            float deltaThrust = TargetThrust - Thrust;
            Thrust = Math.Sign(deltaThrust) * Math.Min(Math.Abs(deltaThrust), ThrustAcceleration * (float)delta);

            // change position
            
        }

        public override void Step(float delta)
        {
            //Console.WriteLine("Position");
            base.Step(delta);
        }
    }
}
