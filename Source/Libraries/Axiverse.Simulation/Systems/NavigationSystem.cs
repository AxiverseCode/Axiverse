using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Simulation.Components;

namespace Axiverse.Simulation.Systems
{
    public class NavigationSystem : System
    {
        public override void Process(Entity entity, float dt)
        {
            var spatial = entity.Spatial;
            var navigation = entity.Get<NavigationComponent>();

            if (navigation != null)
            {
                var direction = navigation.Destination - spatial.Position;
                var distance = direction.Length();
                var velocity = navigation.MaximumVelocity;

                if (distance < navigation.MaximumVelocity * dt)
                {
                    velocity = distance / dt;
                }
                
                spatial.Velocity = direction.OfLength(velocity);

                Console.WriteLine("===== " + distance + " @ " + velocity);

                if (distance < 0.1f)
                {
                    Console.WriteLine("==== Arrived");
                    navigation.Destination = Functions.Random.NextVector3(-100, 100);
                }
            }
        }
    }
}
