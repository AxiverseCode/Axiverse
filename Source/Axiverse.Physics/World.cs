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
    public class World
    {
        public List<RigidBody> RigidBodies { get; }
        private readonly List<RigidBody> m_rigidBodies = new List<RigidBody>();

        public BruteForceFilter BroadPhase { get; set; }
        public CollisionDispatcher NarrowPhase { get; set; }

        private float Timestep { get; set; }

        public World()
        {
            RigidBodies = m_rigidBodies;
            BroadPhase = new BruteForceFilter();
        }

        public void Step(float timestep)
        {
            Timestep = timestep;
            
            // don't integrate if no time has passed.
            if (timestep == 0.0f) return;
            if (timestep < 0.0f) throw new ArgumentException("Timestep must be positive.", "timestep");
            
            m_rigidBodies.ForEach(rigidBody => rigidBody.OnStepping());

            // update contacts
            {
                // for each arbiter update contacts
                // remove arbiters if there are no more contacts
            }

            // remove arbiters (interaction between two bodies)

            // collision broadphase(s) -> pair
            var pairs = BroadPhase.Detect();

            // collision narrowphase(s) -> manifold
            var manifolds = NarrowPhase.Collide(pairs);

            // add manifold (contact pairs) from collision

            // apply constraints

            // check deactivation
            {
                // if movement in an island is slow enough deactivate it

                // preserve contacts so that waking up is stable
            }

            IntegrateForces();

            Integrate();
            // n-body gravitation calculation

            // handle arbiter
            {
                // for each island
                // iterate through contacts and set up
                // iterate through contacts and solve/iterate
            }

            // integrate
            m_rigidBodies.ForEach(rigidBody => rigidBody.OnStepped());

        }



        private void IntegrateForces()
        {
            foreach (var body in m_rigidBodies)
            {
                if (!body.IsStatic && body.IsActive)
                {
                    // linear integration
                    body.LinearVelocity += body.Force * body.InverseMass;

                    // angular integration
                    if (!body.IsParticle)
                    {
                        body.AngularVelocity += body.Torque * Timestep * body.InverseInertiaWorld;
                    }
                }

                body.Force = Vector3.Zero;
                body.Torque = Vector3.Zero;
            }
        }

        private void Integrate()
        {
            foreach (var body in m_rigidBodies)
            {
                if (body.IsStatic || !body.IsActive) continue;
                
                // linear integraion
                body.Position += body.LinearVelocity * Timestep;

                // angular integration
                if (!body.IsParticle)
                {
                    Vector3 axis;
                    float angle = body.AngularVelocity.Length();

                    axis = body.AngularVelocity * (float)Math.Sin(angle * Timestep / 2);
                    Quaternion dorn = new Quaternion(axis, (float)Math.Cos(angle * Timestep / 2));
                    Quaternion ornA = Quaternion.FromMatrix(body.Orientation);

                    dorn = dorn * ornA;
                    dorn.Normalize();

                    body.Orientation = Matrix3.FromQuaternion(dorn);
                }

                body.DeriveProperties();
            }
        }

    }
}
