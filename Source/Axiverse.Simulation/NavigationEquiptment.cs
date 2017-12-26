using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public class NavigationEquiptment : Equiptment
    {
        // Mathematical Models for a Missile Autopilot Design
        // http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.503.2502&rep=rep1&type=pdf  

        public bool FlightAssist { get; set; }

        // maximum torque
        public float MaximumTorque { get; set; }

        // maximum thrust
        public float MaximumThrust { get; set; }

        private readonly VectorPid angularVelocityController = new VectorPid(33.7766f, 0, 0.2553191f);
        private readonly VectorPid headingController = new VectorPid(9.244681f, 0, 0.06382979f);

        public override void Step(float delta)
        {
            // https://en.wikipedia.org/wiki/PID_controller
            // https://forum.unity.com/threads/spaceship-control-using-pid-controllers.191755/

            //// torque towards target
            Vector3 targetPoint = new Vector3(500, 400, 300);

            //// apply thrust - won't take velocity into account, but whatever
            //Vector3 targetOrientation = new Vector3(1, 0, 0);

            //Quaternion target = Quaternion.LookAt(Vector3.UnitY, Vector3.UnitX); //Quaternion.Identity;
            //Quaternion current = Entity.AngularPosition;

            //Quaternion deltaOrientation = target * current.Inverse();
            //Vector3 euler = Quaternion.ToEuler(deltaOrientation);

            //Vector3 currentVelocity = Entity.AngularPosition.InverseTransform(Entity.AngularVelocity);
            //Vector3 difference = euler - Entity.AngularVelocity;
            ////Quaternion deltaVelocity = deltaOrientation * Quaternion.FromEuler(Entity.AngularVelocity).Inverse();
            ////difference = Quaternion.ToEuler(deltaOrientation);

            ////Entity.AngularVelocity = new Vector3();
            //Entity.ResetForces();
            //Entity.ApplyTorque(difference.Normalize());



            Entity.ResetForces();

            // Reduce angular velocity towards 0
            var angularVelocityError = -Entity.AngularVelocity;

            var angularVelocityCorrection = angularVelocityController.Update(angularVelocityError, delta);
            //Entity.ApplyTorque(angularVelocityCorrection);

            var desiredHeading = targetPoint - Entity.LinearPosition;
            var currentHeading = Entity.AngularPosition.Transform(Vector3.UnitY);

            var headingError = Vector3.Cross(currentHeading, desiredHeading).Normal();
            var headingCorrection = headingController.Update(headingError, delta);

            //Entity.ApplyTorque(headingCorrection);

            var appliedTorque = angularVelocityCorrection + headingCorrection;
            if (appliedTorque.Length() > 1)
            {
                appliedTorque.SetLength(1);
            }
            Entity.ApplyTorque(appliedTorque);

            // Primary thrusters
            // a = (2 (-t v_0 + x - x_0))/t^2
            var heading = Entity.AngularPosition.Transform(Vector3.UnitY);
            var thrust = Vector3.Dot((desiredHeading - Entity.LinearVelocity / 10).Normal(), heading);
            Entity.ApplyCentralForce(heading.OfLength(Math.Min(Math.Max(thrust, -1), 1)));

            base.Step(delta);
            //Console.WriteLine($"\tNavigating {headingCorrection}\n\tHeading {heading}");
            //System.Diagnostics.Debug.WriteLine($"{Entity.LinearPosition.X}\t{Entity.LinearPosition.Y}\t{Entity.LinearPosition.Z}");
        }

        public class VectorPid
        {
            public float pFactor, iFactor, dFactor;

            private Vector3 integral;
            private Vector3 lastError;

            public VectorPid(float pFactor, float iFactor, float dFactor)
            {
                this.pFactor = pFactor;
                this.iFactor = iFactor;
                this.dFactor = dFactor;
            }

            public Vector3 Update(Vector3 error, float Δtime)
            {
                integral += error * Δtime;
                var derivative = (error - lastError) / Δtime;
                lastError = error;
                return error * pFactor
                    + integral * iFactor
                    + derivative * dFactor;
            }
        }
    }
}
