using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;

namespace Axiverse.Simulation
{
    public class Body
    {
        // static or kinematic object

        private Matrix3 inverseInertiaTensorWorld;
        private Matrix3 inverseInertiaLocal; // maybe nullable when dirty?
        private Matrix3 inertiaLocal = Matrix3.Identity;// InertialTensors.FromSphere(1, 1); // not used in calc

        // position and orientation and velocities
        private Vector3 linearPosition;
        private Quaternion angularPosition = Quaternion.Identity;
        private Vector3 linearVelocity;
        private Vector3 angularVelocity;

        // artificial factor which forces are scaled
        private Vector3 linearFactor = Vector3.One;
        private Vector3 angularFactor = Vector3.One;

        // artificial factor at which velocities are attenuated
        private Vector3 linearDampening = Vector3.One;
        private Vector3 angularDampening = Vector3.One;

        private float mass = 1; // not used in calc
        private float inverseMass = 1;

        // can be used as per frame accumulators if cleared
        private Vector3 totalForce;
        private Vector3 totalTorque;

 
        public Vector3 LinearPosition { get => linearPosition; set => linearPosition = value; }
        public Quaternion AngularPosition { get => angularPosition; set => angularPosition = value; }
        public Vector3 LinearVelocity => linearVelocity;
        public Vector3 AngularVelocity { get => angularVelocity; set => angularVelocity = value; }

        public float Mass
        {
            get => mass;
            set
            {
                mass = value;
                inverseMass = (mass == 0f) ? 0f : 1f / value;
            }
        }

        /*
        
        // 𝑣 = 𝑣₀ + 𝑎𝑡
        // 𝑥 = 𝑥₀ + 𝑎𝑡
        // 𝑥 = 𝑣₀𝑡 + ½𝑎𝑡²

        float m;

        private Vector3 x; // position
        private Vector3 v; // velocity
        private Vector3 a; // acceleration

        private Quaternion θ; // angular position
        private Vector3 ω; // angular velocity
        private Vector3 α; // angular acceleration

        private Matrix3 I; // moment of inertia
        private Matrix3 inverseI; // inverse moment of inertia
        private Matrix3 inverseIθ;

        private float F; // force
        private float τ; // torque
        */

        public Body()
        {
            Random random = new Random();
            angularPosition = new Quaternion((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            angularPosition.Normalize();
        }

        public void Integrate(float delta)
        {
            UpdateTensor();

            IntegrateVelocities(delta);
            IntegrateTransform(delta);

            //Console.WriteLine($"Position {linearPosition} Orientation {angularPosition}");
            //Console.WriteLine($"Velocity {linearVelocity.Length()} Orientation {angularVelocity.Length()}");
        }

        private void IntegrateVelocities(float delta)
        {
            // linear
            Vector3 linearAcceleration = totalForce * inverseMass;
            linearVelocity = linearVelocity + linearAcceleration * delta;

            // angular
            angularVelocity += Matrix3.Transform(inverseInertiaTensorWorld, totalTorque) * delta;

            // dampening
            linearVelocity = linearVelocity * linearDampening;
            angularVelocity = angularVelocity * angularDampening;
        }

        private void IntegrateTransform(float delta)
        {
            // linear
            linearPosition = linearPosition + linearVelocity * delta;

            // angular
            //angularPosition = angularPosition + (new Quaternion(angularVelocity, 0) * angularPosition) * (delta * 0.5f);
            //angularPosition.Normalize();

            float angle = angularVelocity.Length();
            Vector3 axis;

            if (angle < 0.001f)
            {
                // use Taylor's expansions of sync function
                axis = angularVelocity * (0.5f * delta - (delta * delta * delta) * 0.020833333333f) * angle * angle;
            }
            else
            {
                axis = angularVelocity * ((float)Math.Sin(0.5f * angle * delta) / angle);
            }

            Quaternion deltaOrientation = new Quaternion(axis, (float)Math.Cos(0.5f * angle * delta));
            angularPosition = (angularPosition * deltaOrientation).Normal();
        }

        private void UpdateTensor()
        {
            Matrix3.Inverse(out inverseInertiaLocal, ref inertiaLocal);
            Matrix3 basis = Matrix3.FromQuaternion(angularPosition);
            //basis.scaled(m_invInertiaLocal) - invInertialLocal as diagnal matrix multipy
            inverseInertiaTensorWorld = basis * inverseInertiaLocal * Matrix3.Transpose(basis);
        }

        /// <summary>
        /// Applies an impulse on the center of the body. Will not induce any angular changes.
        /// </summary>
        /// <param name="impulse"></param>
        public void ApplyCentralImpulse(Vector3 impulse)
        {
            // linearVelocity = linearVelocity + impulse * inverseMass * linearFactor
            linearVelocity = linearVelocity + impulse * linearFactor * inverseMass;
        }

        /// <summary>
        /// Applies a torque impulse. Will not induce any linear changes.
        /// </summary>
        /// <param name="torque"></param>
        public void ApplyTorqueImpulse(Vector3 torque)
        {
            // angularVelocity = angularVelocity + inverseInertiaTensorWorld * torque * angularFactor
            angularVelocity = angularVelocity + Matrix3.Transform(inverseInertiaTensorWorld, torque) * angularFactor;
        }

        public void ApplyImpulse(Vector3 impulse, Vector3 localPosition)
        {
            if (inverseMass != 0)
            {
                ApplyCentralImpulse(impulse);
                // if (angularFactor != 0)                
                ApplyTorqueImpulse(localPosition % impulse * linearFactor);

            }
        }

        public void ApplyTorque(Vector3 torque)
        {
            totalTorque += torque * angularFactor;
        }

        public void ApplyCentralForce(Vector3 force)
        {
            totalForce += force * linearFactor;
        }

        public void ApplyForce(Vector3 force, Vector3 localPosition)
        {
            ApplyCentralForce(force);
            ApplyTorque(localPosition % force * linearFactor);
        }

        public void ResetForces()
        {
            totalForce.Set(0);
            totalTorque.Set(0);
        }

        private Vector3 GetLocalPointVelocity(Vector3 localVector)
        {
            return linearVelocity + angularVelocity % localVector;
        }

        //public float CalculateImpluseDenominator(Vector3 position, Vector3 normal)
        //{
        //    Vector3 r = position /* -  center of mass*/;
        //    Vector3 c = Vector3.Cross(r, normal);
        //    Vector3 v = Vector3.Cross(Matrix3.Transform(c, inverseInertiaWorld), r);

        //    return inverseMass + Vector3.Dot(normal, v);
        //}
    }
}
