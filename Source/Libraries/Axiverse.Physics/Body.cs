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
        private Matrix3 inverseInertiaTensorWorld = Matrix3.Identity;
        private Matrix3 inverseInertiaLocal = Matrix3.Identity; // maybe nullable when dirty?
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
        public Vector3 totalForce;
        public Vector3 totalTorque;

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
        /// Gets or sets the linear dampening factor for attenuating linear physics.
        /// </summary>
        public Vector3 LinearDampening
        {
            get => linearDampening;
            set => linearDampening = value;
        }

        /// <summary>
        /// Gets or sets the angular dampening factor for attenuating angular physics.
        /// </summary>
        public Vector3 AngularDampening
        {
            get => angularDampening;
            set => angularDampening = value;
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

        /// <summary>
        /// Gets or sets if the body is static. Static bodies cannot be acted upon but can act on
        /// other bodies for collisions.
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Gets or sets if the body is active. Active bodies are updated and can be deactivated
        /// if the body comes to rest and activated if a force acts on it. A body at rest stays at
        /// rest.
        /// </summary>
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
            // This stored angularVelocity in local space
            // angularPosition = (angularPosition * deltaOrientation).Normal();
            angularPosition = (deltaOrientation * angularPosition).Normal();

            Requires.IsNotNaN(AngularPosition);
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
        /// <param name="globalCentralImpulse">Impulse in global space.</param>
        public void ApplyGlobalCentralImpulse(Vector3 globalCentralImpulse)
        {
            linearVelocity = linearVelocity + globalCentralImpulse * linearFactor * inverseMass;
        }

        /// <summary>
        /// Applies an impulse on the center of the body. Will not cause any linear changes.
        /// </summary>
        /// <param name="momentaryGlobalCentralForce"></param>
        /// <param name="deltaTime"></param>
        public void ApplyGlobalCentralImpulse(Vector3 momentaryGlobalCentralForce, float deltaTime)
        {
            ApplyGlobalCentralImpulse(momentaryGlobalCentralForce * deltaTime);
        }

        /// <summary>
        /// Applies an impulse on the center of the body. Will not cause any linear changes.
        /// </summary>
        /// <param name="localCentralImpulse">Impulse in local space.</param>
        public void ApplyLocalCentralImpulse(Vector3 localCentralImpulse)
        {
            ApplyGlobalCentralImpulse(AngularPosition.Transform(localCentralImpulse));
        }

        /// <summary>
        /// Applies an impulse on the center of the body. Will not cause any linear changes.
        /// </summary>
        /// <param name="momentaryLocalCentralForce"></param>
        /// <param name="deltaTime"></param>
        public void ApplyLocalCentralImpulse(Vector3 momentaryLocalCentralForce, float deltaTime)
        {
            ApplyLocalCentralImpulse(momentaryLocalCentralForce * deltaTime);
        }

        /// <summary>
        /// Applies a torque impulse on the body. Will only cause any angular changes.
        /// </summary>
        /// <param name="globalTorqueImpulse">Torque impulse in global space.</param>
        public void ApplyGlobalTorqueImpulse(Vector3 globalTorqueImpulse)
        {
            angularVelocity = angularVelocity +
                Matrix3.Transform(inverseInertiaTensorWorld, globalTorqueImpulse) * angularFactor;
        }

        /// <summary>
        /// Applies a torque impulse on the body. Will only cause any angular changes.
        /// </summary>
        /// <param name="momentaryGlobalTorque"></param>
        /// <param name="deltaTime"></param>
        public void ApplyGlobalTorqueImpulse(Vector3 momentaryGlobalTorque, float deltaTime)
        {
            ApplyGlobalTorqueImpulse(momentaryGlobalTorque * deltaTime);
        }

        /// <summary>
        /// Applies a torque impulse on the body. Will only cause any angular changes.
        /// </summary>
        /// <param name="localTorqueImpulse">Torque impulse in local space.</param>
        public void ApplyLocalTorqueImpulse(Vector3 localTorqueImpulse)
        {
            ApplyGlobalTorqueImpulse(AngularPosition.Transform(localTorqueImpulse));
        }

        /// <summary>
        /// Applies a torque impulse on the body. Will only cause any angular changes.
        /// </summary>
        /// <param name="momentaryLocalTorque"></param>
        /// <param name="deltaTime"></param>
        public void ApplyLocalTorqueImpulse(Vector3 momentaryLocalTorque, float deltaTime)
        {
            ApplyLocalTorqueImpulse(momentaryLocalTorque * deltaTime);
        }

        /// <summary>
        /// Applies a central force on the body. Will only apply linear forces.
        /// </summary>
        /// <param name="globalCentralForce"></param>
        public void AccumulateGlobalCentralForce(Vector3 globalCentralForce)
        {
            totalForce += globalCentralForce * linearFactor;
        }

        /// <summary>
        /// Applies a central force on the body. Will only apply linear forces.
        /// </summary>
        /// <param name="localCentralForce"></param>
        public void AccumulateLocalCentralForce(Vector3 localCentralForce)
        {
            AccumulateGlobalCentralForce(AngularPosition.Transform(localCentralForce));
        }

        /// <summary>
        /// Applies a torque on the body in global space. Will only apply angular torque.
        /// </summary>
        /// <param name="globalTorque"></param>
        public void AccumulateGlobalTorque(Vector3 globalTorque)
        {
            totalTorque += globalTorque * angularFactor;
        }

        /// <summary>
        /// Applies a torque on the body in local space. Will only apply angular torque.
        /// </summary>
        /// <param name="localTorque"></param>
        public void AccumulateLocalTorque(Vector3 localTorque)
        {
            AccumulateGlobalTorque(AngularPosition.Transform(localTorque));
        }

        /// <summary>
        /// Applies a force at a local position on the body. Can cause linear and/or angular
        /// changes.
        /// </summary>
        /// <param name="globalForce"></param>
        /// <param name="relativeGlobalPosition"></param>
        public void AccumulateForce(Vector3 globalForce, Vector3 relativeGlobalPosition)
        {
            AccumulateGlobalCentralForce(globalForce);
            AccumulateGlobalTorque(relativeGlobalPosition % globalForce * linearFactor);
        }

        /// <summary>
        /// Applies an impulse at a local position on the body. Can cause linear and/or angular
        /// changes.
        /// </summary>
        /// <param name="globalImpulse"></param>
        /// <param name="localPosition"></param>
        public void ApplyImpulse(Vector3 globalImpulse, Vector3 localPosition)
        {
            if (inverseMass != 0)
            {
                ApplyGlobalCentralImpulse(globalImpulse);
                // if (angularFactor != 0)                
                ApplyGlobalTorqueImpulse(localPosition % globalImpulse * linearFactor);

            }
        }

        /// <summary>
        /// Clears both the force and torques applied to the body.
        /// </summary>
        public void ClearForces()
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
        /// Calculates the impulse denominator.
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

        public override string ToString()
        {
            return $"Body 𝑥:{LinearPosition}, ω:{AngularPosition}";
        }
    }
}
