using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;
using Axiverse.Physics.Collision;
using Axiverse.Physics.Filters;

namespace Axiverse.Physics
{
    /// <summary>
    /// A physics simulation world.
    /// </summary>
    public class World
    {
        /// <summary>
        /// Gets the list of bodies in this world.
        /// </summary>
        public List<Body> Bodies { get; }
        private readonly List<Body> m_rigidBodies = new List<Body>();

        /// <summary>
        /// Gets or sets the board phase filter.
        /// </summary>
        public BruteForceFilter BroadPhase { get; set; }

        /// <summary>
        /// Gets or sets the narrow phase collision dispatcher.
        /// </summary>
        public CollisionDispatcher NarrowPhase { get; set; }

        /// <summary>
        /// Gets or sets the default timestep.
        /// </summary>
        private float Timestep { get; set; }

        public World()
        {
            Bodies = m_rigidBodies;
            BroadPhase = new BruteForceFilter(this);
        }

        public void Step(float timestep)
        {
            Timestep = timestep;

            // don't integrate if no time has passed.
            if (timestep == 0.0f) return;
            if (timestep < 0.0f) throw new ArgumentException("Timestep must be positive.", "timestep");

            //m_rigidBodies.ForEach(rigidBody => rigidBody.OnStepping());

            // update contacts
            {
                // for each arbiter update contacts
                // remove arbiters if there are no more contacts
            }

            // remove arbiters (interaction between two bodies)

            // collision broadphase(s) -> pair
            var pairs = BroadPhase.Detect();
            if (pairs.Count != 0)
            {
                //Console.WriteLine("Collision pairs: {0}, pairs.Count);
            }

            // collision narrowphase(s) -> manifold
            //var manifolds = NarrowPhase.Collide(pairs);

            // add manifold (contact pairs) from collision

            // apply constraints

            // check deactivation
            {
                // if movement in an island is slow enough deactivate it

                // preserve contacts so that waking up is stable
            }

            foreach (var body in m_rigidBodies)
            {
                body.Integrate(timestep);
            }

            // n-body gravitation calculation

            // handle arbiter
            {
                // for each island
                // iterate through contacts and set up
                // iterate through contacts and solve/iterate
            }

            // integrate
            // m_rigidBodies.ForEach(rigidBody => rigidBody.OnStepped());

        }
    }
}
