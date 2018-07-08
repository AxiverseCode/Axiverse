using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Physics.Shapes;

namespace Axiverse.Physics
{
    /// <summary>
    /// Represents a rigid body.
    /// </summary>
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

        // collision shapes
        private Shape collisionShape;

        /// <summary>
        /// Gets or sets the linear position of the body.
        /// </summary>
        public Vector3 LinearPosition
        {
            get => linearPosition;
            set => linearPosition = value;
        }

        /// <summary>
        /// Gets or sets the angular position of the body.
        /// </summary>
        public Quaternion AngularPosition
        {
            get => angularPosition;
            set => angularPosition = value;
        }

        /// <summary>
        /// Gets or sets the linear velocity of the body.
        /// </summary>
        public Vector3 LinearVelocity
        {
            get => linearVelocity;
            set => linearVelocity = value;
        }

        /// <summary>
        /// Gets or sets the angular velociety of the body.
        /// </summary>
        public Vector3 AngularVelocity
        {
            get => angularVelocity;
            set => angularVelocity = value;
        }

        /// <summary>
        /// Gets or sets the mass of the body.
        /// </summary>
        public float Mass
        {
            get => mass;
            set
            {
                mass = value;
                inverseMass = (mass == 0f) ? 0f : 1f / value;
            }
        }

        /// <summary>
        /// Gets or sets the collision shape of the body.
        /// </summary>
        public Shape CollisionShape
        {
            get => collisionShape;
            set => collisionShape = value;
        }

        public bool IsStatic { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        /// Constructs a body.
        /// </summary>
        public Body()
        {
        }

        /// <summary>
        /// Integrates the forces over the specified timestep.
        /// </summary>
        /// <param name="delta">The change in time.</param>
        public void Integrate(float delta)
        {
            UpdateInertialTensor();

            IntegrateVelocities(delta);
            IntegrateTransform(delta);
        }

        /// <summary>
        /// Compute the acceleration forces and integrate the linear and angular velocities.
        /// </summary>
        /// <param name="delta">The change in time.</param>
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

        /// <summary>
        /// Compute the final transform by integrating the velocities.
        /// </summary>
        /// <param name="delta">The change in time.</param>
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
                axis = angularVelocity *
                    (0.5f * delta - (delta * delta * delta) * 0.020833333333f) *
                    angle * angle;
            }
            else
            {
                axis = angularVelocity * ((float)Math.Sin(0.5f * angle * delta) / angle);
            }

            Quaternion deltaOrientation = new Quaternion(axis, (float)Math.Cos(0.5f * angle * delta));
            angularPosition = (angularPosition * deltaOrientation).Normal();
        }

        /// <summary>
        /// Update the inertial tensor so that it matches the orientation of the body.
        /// </summary>
        private void UpdateInertialTensor()
        {
            Matrix3.Inverse(out inverseInertiaLocal, ref inertiaLocal);
            Matrix3 basis = Matrix3.FromQuaternion(angularPosition);
            //basis.scaled(m_invInertiaLocal) - invInertialLocal as diagnal matrix multipy
            inverseInertiaTensorWorld = basis * inverseInertiaLocal * Matrix3.Transpose(basis);
        }

        /// <summary>
        /// Applies an impulse on the center of the body. Will not cause any linear changes.
        /// </summary>
        /// <param name="impulse"></param>
        public void ApplyCentralImpulse(Vector3 impulse)
        {
            // linearVelocity = linearVelocity + impulse * inverseMass * linearFactor
            linearVelocity = linearVelocity + impulse * linearFactor * inverseMass;
        }

        /// <summary>
        /// Applies a torque impulse on the body. Will only cause any angular changes.
        /// </summary>
        /// <param name="torque"></param>
        public void ApplyTorqueImpulse(Vector3 torque)
        {
            // angularVelocity = angularVelocity + inverseInertiaTensorWorld * torque * angularFactor
            angularVelocity = angularVelocity +
                Matrix3.Transform(inverseInertiaTensorWorld, torque) * angularFactor;
        }

        /// <summary>
        /// Applies an impulse at a local position on the body. Can cause linear and/or angular
        /// changes.
        /// </summary>
        /// <param name="impulse"></param>
        /// <param name="localPosition"></param>
        public void ApplyImpulse(Vector3 impulse, Vector3 localPosition)
        {
            if (inverseMass != 0)
            {
                ApplyCentralImpulse(impulse);
                // if (angularFactor != 0)                
                ApplyTorqueImpulse(localPosition % impulse * linearFactor);

            }
        }

        /// <summary>
        /// Applies a torque on the body. Will only apply angular torque.
        /// </summary>
        /// <param name="torque"></param>
        public void ApplyTorque(Vector3 torque)
        {
            totalTorque += torque * angularFactor;
        }

        /// <summary>
        /// Applies a central force on the body. Will only apply linear forces.
        /// </summary>
        /// <param name="force"></param>
        public void ApplyCentralForce(Vector3 force)
        {
            totalForce += force * linearFactor;
        }

        /// <summary>
        /// Applies a force at a local position on the body. Can cause linear and/or angular
        /// changes.
        /// </summary>
        /// <param name="force"></param>
        /// <param name="localPosition"></param>
        public void ApplyForce(Vector3 force, Vector3 localPosition)
        {
            ApplyCentralForce(force);
            ApplyTorque(localPosition % force * linearFactor);
        }

        /// <summary>
        /// Resets both the force and torques applied to the body.
        /// </summary>
        public void ResetForces()
        {
            totalForce.Set(0);
            totalTorque.Set(0);
        }

        /// <summary>
        /// Calculates the local velocity at a local position on the body.
        /// </summary>
        /// <param name="localPosition"></param>
        /// <returns></returns>
        public Vector3 GetLocalPointVelocity(Vector3 localPosition)
        {
            return linearVelocity + angularVelocity % localPosition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public float CalculateImpluseDenominator(Vector3 position, Vector3 normal)
        {
            Vector3 r = position /* -  center of mass*/;
            Vector3 c = Vector3.Cross(r, normal);
            Vector3 v = Vector3.Cross(Matrix3.Transform(c, inverseInertiaTensorWorld), r);

            return inverseMass + Vector3.Dot(normal, v);
        }
    }
}
