using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;

namespace Axiverse.Physics
{
    public class RigidBody
    {
        public float Mass; // kg

        internal Matrix3 Inertia;
        internal Matrix3 InverseInertia;
        internal Matrix3 InverseInertiaWorld;

        internal Matrix3 Orientation;
        internal Matrix3 InverseOrientation;

        public Vector3 Position;
        internal Vector3 LinearVelocity;
        internal Vector3 AngularVelocity;

        internal float InverseMass;

        internal bool IsActive { get; set; }
        internal bool IsStatic { get; set; }

        internal Vector3 Force;
        internal Vector3 Torque;

        internal bool IsParticle { get; set; }

        public void OnStepping()
        {

        }

        public void OnStepped()
        {

        }

        public void DeriveProperties()
        {
            // derive everything from orientation, inertia
        }

        public void Integrate(float timestep)
        {
            Position += LinearVelocity * timestep;
        }

        public Vector3 GetLocalPointVelocity(Vector3 localVector)
        {
            return LinearVelocity + Vector3.Cross(AngularVelocity, localVector);
        }

        public float CalculateImpluseDenominator(Vector3 position, Vector3 normal)
        {
            Vector3 r = position /* -  center of mass*/;
            Vector3 c = Vector3.Cross(r, normal);
            Vector3 v = Vector3.Cross(Matrix3.Transform(c, InverseInertiaWorld), r);

            return InverseMass + Vector3.Dot(normal, v);
        }

        public void ApplyImpulse(Vector3 impulse, Vector3 localPosition)
        {
            if (InverseMass != 0)
            {
                ApplyImpulse(impulse);
                ApplyTorqueImpulse(Vector3.Cross(localPosition, impulse));
            }
        }

        public void ApplyImpulse(Vector3 impulse)
        {
            LinearVelocity += impulse * InverseMass;
        }

        public void ApplyTorqueImpulse(Vector3 torqueImpulse)
        {
            AngularVelocity += Matrix3.Transform(InverseInertiaWorld, torqueImpulse);
        }
    }
}
