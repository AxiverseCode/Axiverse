using Axiverse.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Behaviors
{
    class Steering
    {
        public static void Seek()
        {
            Vector3 target = Vector3.One;
            Vector3 position = Vector3.One;
            Vector3 velocity = Vector3.One;
            float maxVelocity = 1;

            Vector3 desiredHeading = (position - target).Normal();
            Vector3 desiredVelocity = desiredHeading.Normal();

            // t = (v_f - v_i) / a := time to decelerate
            // x = v_i + 0.5 * a * t ^ 2 := distance traveled over that time
        }

        public static Vector3 Arrival(Vector3 target, Body body, float maxVelocity, float maxDeceleration)
        {
            Vector3 position = body.LinearPosition;
            Vector3 velocity = body.LinearVelocity;

            Vector3 targetOffset = target - position;
            float distance = targetOffset.Length();

            float rampedSpeed = maxVelocity * (distance / SlowingDistance(velocity.Length(), maxDeceleration));
            float clippedSpeed = Math.Max(rampedSpeed, maxVelocity);

            Vector3 desiredVelocity = clippedSpeed / distance * targetOffset; // clippedSpeed * targetOffset.Normal();
            Vector3 steering = desiredVelocity - velocity;

            return steering;
        }

        
        /// <summary>
        /// Calculates the distance to start slowing with the given starting velocity and 
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="deceleration"></param>
        /// <returns></returns>
        public static float SlowingDistance(float velocity, float deceleration)
        {
            return 0.5f * velocity * velocity / deceleration + velocity;
        }

        /// <summary>
        /// Calculates the separation vector by repelling the agent from all neighbors
        /// exponentially scaled by distance.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="position"></param>
        /// <param name="neighbors"></param>
        /// <returns></returns>
        public static Vector3 Separation(float radius, Vector3 position, IEnumerable<Body> neighbors)
        {
            Vector3 desire = Vector3.Zero;

            foreach (var body in neighbors)
            {
                Vector3 offset = body.LinearPosition - position;
                float scale = 1 / offset.LengthSquared();
                desire -= offset * scale;
            }

            return desire;

        }

        /// <summary>
        /// Calculates the cohesion linear vector by finding the average position of all the
        /// neighboring bodies and the difference between that and that of the agent.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="position"></param>
        /// <param name="neighbors"></param>
        /// <returns></returns>
        public static Vector3 Cohesion(float radius, Vector3 position, IEnumerable<Body> neighbors)
        {
            Vector3 sum = Vector3.Zero;
            int count = 0;

            foreach (var body in neighbors)
            {
                sum += body.LinearPosition;
                count++;
            }

            return (sum / count) - position;
        }

        public static Vector3 Center(IEnumerable<Body> neighbors)
        {
            Vector3 sum = Vector3.Zero;
            int count = 0;

            foreach (var body in neighbors)
            {
                sum += body.LinearPosition;
                count++;
            }

            return sum / count;
        }

        /// <summary>
        /// Calculates the alignment angular vector by finding the average angular position of all
        /// the neightbors and the difference between that and that of the agent.
        /// </summary>
        /// <param name="neighbors"></param>
        public static Vector3 Alignment(Quaternion orientation, IEnumerable<Body> neighbors)
        {
            var average = Quaternion.Average(neighbors.Select(b=>b.AngularPosition));
            average = Quaternion.Identity;
            return Quaternion.ToEuler((average * orientation.Inverse()).Normal());
        }

        public static void Orbit()
        {
            // we have to see if the maximum speed or steering force is the limiting factor

            // travel towards the tangential approach point.
        }
    }
}
