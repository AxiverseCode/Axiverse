using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigationComponent : Component
    {
        // https://math.stackexchange.com/questions/1415798/solving-taking-time-with-acceleration-deceleration-and-speed

        public Vector3 Destination { get; set; }

        public float MaximumVelocity { get; set; } = 50;

        public override Component Clone()
        {
            return new NavigationComponent();
        }
    }
}
