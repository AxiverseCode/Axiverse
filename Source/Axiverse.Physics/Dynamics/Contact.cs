using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Physics.Dynamics
{

    /*
    public class ContactSettings
    {
        public enum MaterialCoefficientMixingType { TakeMaximum, TakeMinimum, UseAverage }

        internal float maximumBias = 10.0f;
        internal float bias = 0.25f;
        internal float minVelocity = 0.001f;
        internal float allowedPenetration = 0.01f;
        internal float breakThreshold = 0.01f;

        internal MaterialCoefficientMixingType materialMode = MaterialCoefficientMixingType.UseAverage;

        public float MaximumBias { get { return maximumBias; } set { maximumBias = value; } }

        public float BiasFactor { get { return bias; } set { bias = value; } }

        public float MinimumVelocity { get { return minVelocity; } set { minVelocity = value; } }

        public float AllowedPenetration { get { return allowedPenetration; } set { allowedPenetration = value; } }

        public float BreakThreshold { get { return breakThreshold; } set { breakThreshold = value; } }

        public MaterialCoefficientMixingType MaterialCoefficientMixing { get { return materialMode; } set { materialMode = value; } }
    }

    /// <summary>
    /// </summary>
    public class Contact : IConstraint
    {
        private ContactSettings settings;

        internal RigidBody body1, body2;

        internal Vector3 normal, tangent;

        internal Vector3 realRelPos1, realRelPos2;
        internal Vector3 relativePos1, relativePos2;
        internal Vector3 p1, p2;

        internal float accumulatedNormalImpulse = 0.0f;
        internal float accumulatedTangentImpulse = 0.0f;

        internal float penetration = 0.0f;
        internal float initialPen = 0.0f;

        private float staticFriction, dynamicFriction, restitution;
        private float friction = 0.0f;

        private float massNormal = 0.0f, massTangent = 0.0f;
        private float restitutionBias = 0.0f;

        private bool newContact = false;

        private bool treatBody1AsStatic = false;
        private bool treatBody2AsStatic = false;


        bool body1IsMassPoint; bool body2IsMassPoint;

        float lostSpeculativeBounce = 0.0f;
        float speculativeVelocity = 0.0f;

        /// <summary>
        /// A contact resource pool.
        /// </summary>
        public static readonly ResourcePool<Contact> Pool =
            new ResourcePool<Contact>();

        private float lastTimeStep = float.PositiveInfinity;

        #region Properties
        public float Restitution
        {
            get { return restitution; }
            set { restitution = value; }
        }

        public float StaticFriction
        {
            get { return staticFriction; }
            set { staticFriction = value; }
        }

        public float DynamicFriction
        {
            get { return dynamicFriction; }
            set { dynamicFriction = value; }
        }

        /// <summary>
        /// The first body involved in the contact.
        /// </summary>
        public RigidBody Body1 { get { return body1; } }

        /// <summary>
        /// The second body involved in the contact.
        /// </summary>
        public RigidBody Body2 { get { return body2; } }

        /// <summary>
        /// The penetration of the contact.
        /// </summary>
        public float Penetration { get { return penetration; } }

        /// <summary>
        /// The collision position in world space of body1.
        /// </summary>
        public Vector3 Position1 { get { return p1; } }

        /// <summary>
        /// The collision position in world space of body2.
        /// </summary>
        public Vector3 Position2 { get { return p2; } }

        /// <summary>
        /// The contact tangent.
        /// </summary>
        public Vector3 Tangent { get { return tangent; } }

        /// <summary>
        /// The contact normal.
        /// </summary>
        public Vector3 Normal { get { return normal; } }
        #endregion

        /// <summary>
        /// Calculates relative velocity of body contact points on the bodies.
        /// </summary>
        /// <param name="relVel">The relative velocity of body contact points on the bodies.</param>
        public Vector3 CalculateRelativeVelocity()
        {
            Vector3 result = Vector3.Cross(body2.AngularVelocity, relativePos2) + body2.LinearVelocity;
            result -= Vector3.Cross(body1.AngularVelocity, relativePos2) + body1.LinearVelocity;
            return result;
        }

        /// <summary>
        /// Solves the contact iteratively.
        /// </summary>
        public void Iterate()
        {
            //body1.LinearVelocity = Vector3.Zero;
            //body2.LinearVelocity = Vector3.Zero;
            //return;

            if (treatBody1AsStatic && treatBody2AsStatic) return;

            float dvx, dvy, dvz;

            dvx = body2.LinearVelocity.X - body1.LinearVelocity.X;
            dvy = body2.LinearVelocity.Y - body1.LinearVelocity.Y;
            dvz = body2.LinearVelocity.Z - body1.LinearVelocity.Z;

            if (!body1IsMassPoint)
            {
                dvx = dvx - (body1.AngularVelocity.Y * relativePos1.Z) + (body1.AngularVelocity.Z * relativePos1.Y);
                dvy = dvy - (body1.AngularVelocity.Z * relativePos1.X) + (body1.AngularVelocity.X * relativePos1.Z);
                dvz = dvz - (body1.AngularVelocity.X * relativePos1.Y) + (body1.AngularVelocity.Y * relativePos1.X);
            }

            if (!body2IsMassPoint)
            {
                dvx = dvx + (body2.AngularVelocity.Y * relativePos2.Z) - (body2.AngularVelocity.Z * relativePos2.Y);
                dvy = dvy + (body2.AngularVelocity.Z * relativePos2.X) - (body2.AngularVelocity.X * relativePos2.Z);
                dvz = dvz + (body2.AngularVelocity.X * relativePos2.Y) - (body2.AngularVelocity.Y * relativePos2.X);
            }

            // this gets us some performance
            if (dvx * dvx + dvy * dvy + dvz * dvz < settings.minVelocity * settings.minVelocity)
            { return; }

            float vn = normal.X * dvx + normal.Y * dvy + normal.Z * dvz;
            float normalImpulse = massNormal * (-vn + restitutionBias + speculativeVelocity);

            float oldNormalImpulse = accumulatedNormalImpulse;
            accumulatedNormalImpulse = oldNormalImpulse + normalImpulse;
            if (accumulatedNormalImpulse < 0.0f) accumulatedNormalImpulse = 0.0f;
            normalImpulse = accumulatedNormalImpulse - oldNormalImpulse;

            float vt = dvx * tangent.X + dvy * tangent.Y + dvz * tangent.Z;
            float maxTangentImpulse = friction * accumulatedNormalImpulse;
            float tangentImpulse = massTangent * (-vt);

            float oldTangentImpulse = accumulatedTangentImpulse;
            accumulatedTangentImpulse = oldTangentImpulse + tangentImpulse;
            if (accumulatedTangentImpulse < -maxTangentImpulse) accumulatedTangentImpulse = -maxTangentImpulse;
            else if (accumulatedTangentImpulse > maxTangentImpulse) accumulatedTangentImpulse = maxTangentImpulse;

            tangentImpulse = accumulatedTangentImpulse - oldTangentImpulse;

            // Apply contact impulse
            Vector3 impulse;
            impulse.X = normal.X * normalImpulse + tangent.X * tangentImpulse;
            impulse.Y = normal.Y * normalImpulse + tangent.Y * tangentImpulse;
            impulse.Z = normal.Z * normalImpulse + tangent.Z * tangentImpulse;

            if (!treatBody1AsStatic)
            {
                body1.LinearVelocity.X -= (impulse.X * body1.InverseMass);
                body1.LinearVelocity.Y -= (impulse.Y * body1.InverseMass);
                body1.LinearVelocity.Z -= (impulse.Z * body1.InverseMass);

                if (!body1IsMassPoint)
                {
                    float num0, num1, num2;
                    num0 = relativePos1.Y * impulse.Z - relativePos1.Z * impulse.Y;
                    num1 = relativePos1.Z * impulse.X - relativePos1.X * impulse.Z;
                    num2 = relativePos1.X * impulse.Y - relativePos1.Y * impulse.X;

                    float num3 =
                        (((num0 * body1.InverseInertiaWorld.M11) +
                        (num1 * body1.InverseInertiaWorld.M21)) +
                        (num2 * body1.InverseInertiaWorld.M31));
                    float num4 =
                        (((num0 * body1.InverseInertiaWorld.M12) +
                        (num1 * body1.InverseInertiaWorld.M22)) +
                        (num2 * body1.InverseInertiaWorld.M32));
                    float num5 =
                        (((num0 * body1.InverseInertiaWorld.M13) +
                        (num1 * body1.InverseInertiaWorld.M23)) +
                        (num2 * body1.InverseInertiaWorld.M33));

                    body1.AngularVelocity.X -= num3;
                    body1.AngularVelocity.Y -= num4;
                    body1.AngularVelocity.Z -= num5;
                }
            }

            if (!treatBody2AsStatic)
            {

                body2.LinearVelocity.X += (impulse.X * body2.InverseMass);
                body2.LinearVelocity.Y += (impulse.Y * body2.InverseMass);
                body2.LinearVelocity.Z += (impulse.Z * body2.InverseMass);

                if (!body2IsMassPoint)
                {

                    float num0, num1, num2;
                    num0 = relativePos2.Y * impulse.Z - relativePos2.Z * impulse.Y;
                    num1 = relativePos2.Z * impulse.X - relativePos2.X * impulse.Z;
                    num2 = relativePos2.X * impulse.Y - relativePos2.Y * impulse.X;

                    float num3 =
                        (((num0 * body2.InverseInertiaWorld.M11) +
                        (num1 * body2.InverseInertiaWorld.M21)) +
                        (num2 * body2.InverseInertiaWorld.M31));
                    float num4 =
                        (((num0 * body2.InverseInertiaWorld.M12) +
                        (num1 * body2.InverseInertiaWorld.M22)) +
                        (num2 * body2.InverseInertiaWorld.M32));
                    float num5 =
                        (((num0 * body2.InverseInertiaWorld.M13) +
                        (num1 * body2.InverseInertiaWorld.M23)) +
                        (num2 * body2.InverseInertiaWorld.M33));

                    body2.AngularVelocity.X += num3;
                    body2.AngularVelocity.Y += num4;
                    body2.AngularVelocity.Z += num5;

                }
            }

        }

        public float AppliedNormalImpulse { get { return accumulatedNormalImpulse; } }
        public float AppliedTangentImpulse { get { return accumulatedTangentImpulse; } }

        /// <summary>
        /// The points in wolrd space gets recalculated by transforming the
        /// local coordinates. Also new penetration depth is estimated.
        /// </summary>
        public void UpdatePosition()
        {
            if (body1IsMassPoint)
            {
                Vector3.Add(ref realRelPos1, ref body1.Position, out p1);
            }
            else
            {
                Vector3.Transform(ref realRelPos1, ref body1.Orientation, out p1);
                Vector3.Add(ref p1, ref body1.Position, out p1);
            }

            if (body2IsMassPoint)
            {
                Vector3.Add(ref realRelPos2, ref body2.Position, out p2);
            }
            else
            {
                Vector3.Transform(ref realRelPos2, ref body2.Orientation, out p2);
                Vector3.Add(ref p2, ref body2.Position, out p2);
            }


            Vector3 dist; Vector3.Subtract(ref p1, ref p2, out dist);
            penetration = Vector3.Dot(ref dist, ref normal);
        }

        /// <summary>
        /// An impulse is applied an both contact points.
        /// </summary>
        /// <param name="impulse">The impulse to apply.</param>
        public void ApplyImpulse(ref Vector3 impulse)
        {
            #region INLINE - HighFrequency
            //Vector3 temp;

            if (!treatBody1AsStatic)
            {
                body1.LinearVelocity.X -= (impulse.X * body1.InverseMass);
                body1.LinearVelocity.Y -= (impulse.Y * body1.InverseMass);
                body1.LinearVelocity.Z -= (impulse.Z * body1.InverseMass);

                float num0, num1, num2;
                num0 = relativePos1.Y * impulse.Z - relativePos1.Z * impulse.Y;
                num1 = relativePos1.Z * impulse.X - relativePos1.X * impulse.Z;
                num2 = relativePos1.X * impulse.Y - relativePos1.Y * impulse.X;

                float num3 =
                    (((num0 * body1.InverseInertiaWorld.M11) +
                    (num1 * body1.InverseInertiaWorld.M21)) +
                    (num2 * body1.InverseInertiaWorld.M31));
                float num4 =
                    (((num0 * body1.InverseInertiaWorld.M12) +
                    (num1 * body1.InverseInertiaWorld.M22)) +
                    (num2 * body1.InverseInertiaWorld.M32));
                float num5 =
                    (((num0 * body1.InverseInertiaWorld.M13) +
                    (num1 * body1.InverseInertiaWorld.M23)) +
                    (num2 * body1.InverseInertiaWorld.M33));

                body1.AngularVelocity.X -= num3;
                body1.AngularVelocity.Y -= num4;
                body1.AngularVelocity.Z -= num5;
            }

            if (!treatBody2AsStatic)
            {

                body2.LinearVelocity.X += (impulse.X * body2.InverseMass);
                body2.LinearVelocity.Y += (impulse.Y * body2.InverseMass);
                body2.LinearVelocity.Z += (impulse.Z * body2.InverseMass);

                float num0, num1, num2;
                num0 = relativePos2.Y * impulse.Z - relativePos2.Z * impulse.Y;
                num1 = relativePos2.Z * impulse.X - relativePos2.X * impulse.Z;
                num2 = relativePos2.X * impulse.Y - relativePos2.Y * impulse.X;

                float num3 =
                    (((num0 * body2.InverseInertiaWorld.M11) +
                    (num1 * body2.InverseInertiaWorld.M21)) +
                    (num2 * body2.InverseInertiaWorld.M31));
                float num4 =
                    (((num0 * body2.InverseInertiaWorld.M12) +
                    (num1 * body2.InverseInertiaWorld.M22)) +
                    (num2 * body2.InverseInertiaWorld.M32));
                float num5 =
                    (((num0 * body2.InverseInertiaWorld.M13) +
                    (num1 * body2.InverseInertiaWorld.M23)) +
                    (num2 * body2.InverseInertiaWorld.M33));

                body2.AngularVelocity.X += num3;
                body2.AngularVelocity.Y += num4;
                body2.AngularVelocity.Z += num5;
            }


            #endregion
        }

        public void ApplyImpulse(Vector3 impulse)
        {
            #region INLINE - HighFrequency
            //Vector3 temp;

            if (!treatBody1AsStatic)
            {
                body1.LinearVelocity.X -= (impulse.X * body1.InverseMass);
                body1.LinearVelocity.Y -= (impulse.Y * body1.InverseMass);
                body1.LinearVelocity.Z -= (impulse.Z * body1.InverseMass);

                float num0, num1, num2;
                num0 = relativePos1.Y * impulse.Z - relativePos1.Z * impulse.Y;
                num1 = relativePos1.Z * impulse.X - relativePos1.X * impulse.Z;
                num2 = relativePos1.X * impulse.Y - relativePos1.Y * impulse.X;

                float num3 =
                    (((num0 * body1.InverseInertiaWorld.M11) +
                    (num1 * body1.InverseInertiaWorld.M21)) +
                    (num2 * body1.InverseInertiaWorld.M31));
                float num4 =
                    (((num0 * body1.InverseInertiaWorld.M12) +
                    (num1 * body1.InverseInertiaWorld.M22)) +
                    (num2 * body1.InverseInertiaWorld.M32));
                float num5 =
                    (((num0 * body1.InverseInertiaWorld.M13) +
                    (num1 * body1.InverseInertiaWorld.M23)) +
                    (num2 * body1.InverseInertiaWorld.M33));

                body1.AngularVelocity.X -= num3;
                body1.AngularVelocity.Y -= num4;
                body1.AngularVelocity.Z -= num5;
            }

            if (!treatBody2AsStatic)
            {

                body2.LinearVelocity.X += (impulse.X * body2.InverseMass);
                body2.LinearVelocity.Y += (impulse.Y * body2.InverseMass);
                body2.LinearVelocity.Z += (impulse.Z * body2.InverseMass);

                float num0, num1, num2;
                num0 = relativePos2.Y * impulse.Z - relativePos2.Z * impulse.Y;
                num1 = relativePos2.Z * impulse.X - relativePos2.X * impulse.Z;
                num2 = relativePos2.X * impulse.Y - relativePos2.Y * impulse.X;

                float num3 =
                    (((num0 * body2.InverseInertiaWorld.M11) +
                    (num1 * body2.InverseInertiaWorld.M21)) +
                    (num2 * body2.InverseInertiaWorld.M31));
                float num4 =
                    (((num0 * body2.InverseInertiaWorld.M12) +
                    (num1 * body2.InverseInertiaWorld.M22)) +
                    (num2 * body2.InverseInertiaWorld.M32));
                float num5 =
                    (((num0 * body2.InverseInertiaWorld.M13) +
                    (num1 * body2.InverseInertiaWorld.M23)) +
                    (num2 * body2.InverseInertiaWorld.M33));

                body2.AngularVelocity.X += num3;
                body2.AngularVelocity.Y += num4;
                body2.AngularVelocity.Z += num5;
            }


            #endregion
        }

        /// <summary>
        /// PrepareForIteration has to be called before <see cref="Iterate"/>.
        /// </summary>
        /// <param name="timestep">The timestep of the simulation.</param>
        public void PrepareForIteration(float timestep)
        {
            float dvx, dvy, dvz;

            dvx = (body2.AngularVelocity.Y * relativePos2.Z) - (body2.AngularVelocity.Z * relativePos2.Y) + body2.LinearVelocity.X;
            dvy = (body2.AngularVelocity.Z * relativePos2.X) - (body2.AngularVelocity.X * relativePos2.Z) + body2.LinearVelocity.Y;
            dvz = (body2.AngularVelocity.X * relativePos2.Y) - (body2.AngularVelocity.Y * relativePos2.X) + body2.LinearVelocity.Z;

            dvx = dvx - (body1.AngularVelocity.Y * relativePos1.Z) + (body1.AngularVelocity.Z * relativePos1.Y) - body1.LinearVelocity.X;
            dvy = dvy - (body1.AngularVelocity.Z * relativePos1.X) + (body1.AngularVelocity.X * relativePos1.Z) - body1.LinearVelocity.Y;
            dvz = dvz - (body1.AngularVelocity.X * relativePos1.Y) + (body1.AngularVelocity.Y * relativePos1.X) - body1.LinearVelocity.Z;

            float kNormal = 0.0f;

            Vector3 rantra = Vector3.Zero;
            if (!treatBody1AsStatic)
            {
                kNormal += body1.InverseMass;

                if (!body1IsMassPoint)
                {

                    // Vector3.Cross(ref relativePos1, ref normal, out rantra);
                    rantra.X = (relativePos1.Y * normal.Z) - (relativePos1.Z * normal.Y);
                    rantra.Y = (relativePos1.Z * normal.X) - (relativePos1.X * normal.Z);
                    rantra.Z = (relativePos1.X * normal.Y) - (relativePos1.Y * normal.X);

                    // Vector3.Transform(ref rantra, ref body1.InverseInertiaWorld, out rantra);
                    float num0 = ((rantra.X * body1.InverseInertiaWorld.M11) + (rantra.Y * body1.InverseInertiaWorld.M21)) + (rantra.Z * body1.InverseInertiaWorld.M31);
                    float num1 = ((rantra.X * body1.InverseInertiaWorld.M12) + (rantra.Y * body1.InverseInertiaWorld.M22)) + (rantra.Z * body1.InverseInertiaWorld.M32);
                    float num2 = ((rantra.X * body1.InverseInertiaWorld.M13) + (rantra.Y * body1.InverseInertiaWorld.M23)) + (rantra.Z * body1.InverseInertiaWorld.M33);

                    rantra.X = num0; rantra.Y = num1; rantra.Z = num2;

                    //Vector3.Cross(ref rantra, ref relativePos1, out rantra);
                    num0 = (rantra.Y * relativePos1.Z) - (rantra.Z * relativePos1.Y);
                    num1 = (rantra.Z * relativePos1.X) - (rantra.X * relativePos1.Z);
                    num2 = (rantra.X * relativePos1.Y) - (rantra.Y * relativePos1.X);

                    rantra.X = num0; rantra.Y = num1; rantra.Z = num2;
                }
            }

            Vector3 rbntrb = Vector3.Zero;
            if (!treatBody2AsStatic)
            {
                kNormal += body2.InverseMass;

                if (!body2IsMassPoint)
                {

                    // Vector3.Cross(ref relativePos1, ref normal, out rantra);
                    rbntrb.X = (relativePos2.Y * normal.Z) - (relativePos2.Z * normal.Y);
                    rbntrb.Y = (relativePos2.Z * normal.X) - (relativePos2.X * normal.Z);
                    rbntrb.Z = (relativePos2.X * normal.Y) - (relativePos2.Y * normal.X);

                    // Vector3.Transform(ref rantra, ref body1.InverseInertiaWorld, out rantra);
                    float num0 = ((rbntrb.X * body2.InverseInertiaWorld.M11) + (rbntrb.Y * body2.InverseInertiaWorld.M21)) + (rbntrb.Z * body2.InverseInertiaWorld.M31);
                    float num1 = ((rbntrb.X * body2.InverseInertiaWorld.M12) + (rbntrb.Y * body2.InverseInertiaWorld.M22)) + (rbntrb.Z * body2.InverseInertiaWorld.M32);
                    float num2 = ((rbntrb.X * body2.InverseInertiaWorld.M13) + (rbntrb.Y * body2.InverseInertiaWorld.M23)) + (rbntrb.Z * body2.InverseInertiaWorld.M33);

                    rbntrb.X = num0; rbntrb.Y = num1; rbntrb.Z = num2;

                    //Vector3.Cross(ref rantra, ref relativePos1, out rantra);
                    num0 = (rbntrb.Y * relativePos2.Z) - (rbntrb.Z * relativePos2.Y);
                    num1 = (rbntrb.Z * relativePos2.X) - (rbntrb.X * relativePos2.Z);
                    num2 = (rbntrb.X * relativePos2.Y) - (rbntrb.Y * relativePos2.X);

                    rbntrb.X = num0; rbntrb.Y = num1; rbntrb.Z = num2;
                }
            }

            if (!treatBody1AsStatic) kNormal += rantra.X * normal.X + rantra.Y * normal.Y + rantra.Z * normal.Z;
            if (!treatBody2AsStatic) kNormal += rbntrb.X * normal.X + rbntrb.Y * normal.Y + rbntrb.Z * normal.Z;

            massNormal = 1.0f / kNormal;

            float num = dvx * normal.X + dvy * normal.Y + dvz * normal.Z;

            tangent.X = dvx - normal.X * num;
            tangent.Y = dvy - normal.Y * num;
            tangent.Z = dvz - normal.Z * num;

            num = tangent.X * tangent.X + tangent.Y * tangent.Y + tangent.Z * tangent.Z;

            if (num != 0.0f)
            {
                num = (float)Math.Sqrt(num);
                tangent.X /= num;
                tangent.Y /= num;
                tangent.Z /= num;
            }

            float kTangent = 0.0f;

            if (treatBody1AsStatic)
            {
                rantra = Vector3.Zero;
            }
            else
            {
                kTangent += body1.InverseMass;

                if (!body1IsMassPoint)
                {
                    // Vector3.Cross(ref relativePos1, ref normal, out rantra);
                    rantra.X = (relativePos1.Y * tangent.Z) - (relativePos1.Z * tangent.Y);
                    rantra.Y = (relativePos1.Z * tangent.X) - (relativePos1.X * tangent.Z);
                    rantra.Z = (relativePos1.X * tangent.Y) - (relativePos1.Y * tangent.X);

                    // Vector3.Transform(ref rantra, ref body1.InverseInertiaWorld, out rantra);
                    float num0 = ((rantra.X * body1.InverseInertiaWorld.M11) + (rantra.Y * body1.InverseInertiaWorld.M21)) + (rantra.Z * body1.InverseInertiaWorld.M31);
                    float num1 = ((rantra.X * body1.InverseInertiaWorld.M12) + (rantra.Y * body1.InverseInertiaWorld.M22)) + (rantra.Z * body1.InverseInertiaWorld.M32);
                    float num2 = ((rantra.X * body1.InverseInertiaWorld.M13) + (rantra.Y * body1.InverseInertiaWorld.M23)) + (rantra.Z * body1.InverseInertiaWorld.M33);

                    rantra.X = num0; rantra.Y = num1; rantra.Z = num2;

                    //Vector3.Cross(ref rantra, ref relativePos1, out rantra);
                    num0 = (rantra.Y * relativePos1.Z) - (rantra.Z * relativePos1.Y);
                    num1 = (rantra.Z * relativePos1.X) - (rantra.X * relativePos1.Z);
                    num2 = (rantra.X * relativePos1.Y) - (rantra.Y * relativePos1.X);

                    rantra.X = num0; rantra.Y = num1; rantra.Z = num2;
                }

            }

            if (treatBody2AsStatic)
            {
                rbntrb = Vector3.Zero;
            }
            else
            {
                kTangent += body2.InverseMass;

                if (!body2IsMassPoint)
                {
                    // Vector3.Cross(ref relativePos1, ref normal, out rantra);
                    rbntrb.X = (relativePos2.Y * tangent.Z) - (relativePos2.Z * tangent.Y);
                    rbntrb.Y = (relativePos2.Z * tangent.X) - (relativePos2.X * tangent.Z);
                    rbntrb.Z = (relativePos2.X * tangent.Y) - (relativePos2.Y * tangent.X);

                    // Vector3.Transform(ref rantra, ref body1.InverseInertiaWorld, out rantra);
                    float num0 = ((rbntrb.X * body2.InverseInertiaWorld.M11) + (rbntrb.Y * body2.InverseInertiaWorld.M21)) + (rbntrb.Z * body2.InverseInertiaWorld.M31);
                    float num1 = ((rbntrb.X * body2.InverseInertiaWorld.M12) + (rbntrb.Y * body2.InverseInertiaWorld.M22)) + (rbntrb.Z * body2.InverseInertiaWorld.M32);
                    float num2 = ((rbntrb.X * body2.InverseInertiaWorld.M13) + (rbntrb.Y * body2.InverseInertiaWorld.M23)) + (rbntrb.Z * body2.InverseInertiaWorld.M33);

                    rbntrb.X = num0; rbntrb.Y = num1; rbntrb.Z = num2;

                    //Vector3.Cross(ref rantra, ref relativePos1, out rantra);
                    num0 = (rbntrb.Y * relativePos2.Z) - (rbntrb.Z * relativePos2.Y);
                    num1 = (rbntrb.Z * relativePos2.X) - (rbntrb.X * relativePos2.Z);
                    num2 = (rbntrb.X * relativePos2.Y) - (rbntrb.Y * relativePos2.X);

                    rbntrb.X = num0; rbntrb.Y = num1; rbntrb.Z = num2;
                }
            }

            if (!treatBody1AsStatic) kTangent += Vector3.Dot(ref rantra, ref tangent);
            if (!treatBody2AsStatic) kTangent += Vector3.Dot(ref rbntrb, ref tangent);
            massTangent = 1.0f / kTangent;

            restitutionBias = lostSpeculativeBounce;

            speculativeVelocity = 0.0f;

            float relNormalVel = normal.X * dvx + normal.Y * dvy + normal.Z * dvz; //Vector3.Dot(ref normal, ref dv);

            if (Penetration > settings.allowedPenetration)
            {
                restitutionBias = settings.bias * (1.0f / timestep) * Math.Max(0.0f, Penetration - settings.allowedPenetration);
                restitutionBias = Math.Clamp(restitutionBias, 0.0f, settings.maximumBias);
                //  body1IsMassPoint = body2IsMassPoint = false;
            }


            float timeStepRatio = timestep / lastTimeStep;
            accumulatedNormalImpulse *= timeStepRatio;
            accumulatedTangentImpulse *= timeStepRatio;

            {
                // Static/Dynamic friction
                float relTangentVel = -(tangent.X * dvx + tangent.Y * dvy + tangent.Z * dvz);
                float tangentImpulse = massTangent * relTangentVel;
                float maxTangentImpulse = -staticFriction * accumulatedNormalImpulse;

                if (tangentImpulse < maxTangentImpulse) friction = dynamicFriction;
                else friction = staticFriction;
            }

            Vector3 impulse;

            // Simultaneos solving and restitution is simply not possible
            // so fake it a bit by just applying restitution impulse when there
            // is a new contact.
            if (relNormalVel < -1.0f && newContact)
            {
                restitutionBias = Math.Max(-restitution * relNormalVel, restitutionBias);
            }

            // Speculative Contacts!
            // if the penetration is negative (which means the bodies are not already in contact, but they will
            // be in the future) we store the current bounce bias in the variable 'lostSpeculativeBounce'
            // and apply it the next frame, when the speculative contact was already solved.
            if (penetration < -settings.allowedPenetration)
            {
                speculativeVelocity = penetration / timestep;

                lostSpeculativeBounce = restitutionBias;
                restitutionBias = 0.0f;
            }
            else
            {
                lostSpeculativeBounce = 0.0f;
            }

            impulse.X = normal.X * accumulatedNormalImpulse + tangent.X * accumulatedTangentImpulse;
            impulse.Y = normal.Y * accumulatedNormalImpulse + tangent.Y * accumulatedTangentImpulse;
            impulse.Z = normal.Z * accumulatedNormalImpulse + tangent.Z * accumulatedTangentImpulse;

            if (!treatBody1AsStatic)
            {
                body1.LinearVelocity.X -= (impulse.X * body1.InverseMass);
                body1.LinearVelocity.Y -= (impulse.Y * body1.InverseMass);
                body1.LinearVelocity.Z -= (impulse.Z * body1.InverseMass);

                if (!body1IsMassPoint)
                {
                    float num0, num1, num2;
                    num0 = relativePos1.Y * impulse.Z - relativePos1.Z * impulse.Y;
                    num1 = relativePos1.Z * impulse.X - relativePos1.X * impulse.Z;
                    num2 = relativePos1.X * impulse.Y - relativePos1.Y * impulse.X;

                    float num3 =
                        (((num0 * body1.InverseInertiaWorld.M11) +
                        (num1 * body1.InverseInertiaWorld.M21)) +
                        (num2 * body1.InverseInertiaWorld.M31));
                    float num4 =
                        (((num0 * body1.InverseInertiaWorld.M12) +
                        (num1 * body1.InverseInertiaWorld.M22)) +
                        (num2 * body1.InverseInertiaWorld.M32));
                    float num5 =
                        (((num0 * body1.InverseInertiaWorld.M13) +
                        (num1 * body1.InverseInertiaWorld.M23)) +
                        (num2 * body1.InverseInertiaWorld.M33));

                    body1.AngularVelocity.X -= num3;
                    body1.AngularVelocity.Y -= num4;
                    body1.AngularVelocity.Z -= num5;

                }
            }

            if (!treatBody2AsStatic)
            {

                body2.LinearVelocity.X += (impulse.X * body2.InverseMass);
                body2.LinearVelocity.Y += (impulse.Y * body2.InverseMass);
                body2.LinearVelocity.Z += (impulse.Z * body2.InverseMass);

                if (!body2IsMassPoint)
                {

                    float num0, num1, num2;
                    num0 = relativePos2.Y * impulse.Z - relativePos2.Z * impulse.Y;
                    num1 = relativePos2.Z * impulse.X - relativePos2.X * impulse.Z;
                    num2 = relativePos2.X * impulse.Y - relativePos2.Y * impulse.X;

                    float num3 =
                        (((num0 * body2.InverseInertiaWorld.M11) +
                        (num1 * body2.InverseInertiaWorld.M21)) +
                        (num2 * body2.InverseInertiaWorld.M31));
                    float num4 =
                        (((num0 * body2.InverseInertiaWorld.M12) +
                        (num1 * body2.InverseInertiaWorld.M22)) +
                        (num2 * body2.InverseInertiaWorld.M32));
                    float num5 =
                        (((num0 * body2.InverseInertiaWorld.M13) +
                        (num1 * body2.InverseInertiaWorld.M23)) +
                        (num2 * body2.InverseInertiaWorld.M33));

                    body2.AngularVelocity.X += num3;
                    body2.AngularVelocity.Y += num4;
                    body2.AngularVelocity.Z += num5;
                }
            }

            lastTimeStep = timestep;

            newContact = false;
        }

        public void TreatBodyAsStatic(RigidBodyIndex index)
        {
            if (index == RigidBodyIndex.RigidBody1) treatBody1AsStatic = true;
            else treatBody2AsStatic = true;
        }


        /// <summary>
        /// Initializes a contact.
        /// </summary>
        /// <param name="body1">The first body.</param>
        /// <param name="body2">The second body.</param>
        /// <param name="point1">The collision point in worldspace</param>
        /// <param name="point2">The collision point in worldspace</param>
        /// <param name="n">The normal pointing to body2.</param>
        /// <param name="penetration">The estimated penetration depth.</param>
        public void Initialize(RigidBody body1, RigidBody body2, ref Vector3 point1, ref Vector3 point2, ref Vector3 n,
            float penetration, bool newContact, ContactSettings settings)
        {
            this.body1 = body1; this.body2 = body2;
            this.normal = n; normal.Normalize();
            this.p1 = point1; this.p2 = point2;

            this.newContact = newContact;

            Vector3.Subtract(ref p1, ref body1.Position, out relativePos1);
            Vector3.Subtract(ref p2, ref body2.Position, out relativePos2);
            Vector3.Transform(ref relativePos1, ref body1.InverseOrientation, out realRelPos1);
            Vector3.Transform(ref relativePos2, ref body2.InverseOrientation, out realRelPos2);

            this.initialPen = penetration;
            this.penetration = penetration;

            body1IsMassPoint = body1.IsParticle;
            body2IsMassPoint = body2.IsParticle;

            // Material Properties
            if (newContact)
            {
                treatBody1AsStatic = body1.IsStatic;
                treatBody2AsStatic = body2.IsStatic;

                accumulatedNormalImpulse = 0.0f;
                accumulatedTangentImpulse = 0.0f;

                lostSpeculativeBounce = 0.0f;

                switch (settings.MaterialCoefficientMixing)
                {
                    case ContactSettings.MaterialCoefficientMixingType.TakeMaximum:
                        staticFriction = Math.Max(body1.Material.staticFriction, body2.Material.staticFriction);
                        dynamicFriction = Math.Max(body1.Material.kineticFriction, body2.Material.kineticFriction);
                        restitution = Math.Max(body1.Material.restitution, body2.Material.restitution);
                        break;
                    case ContactSettings.MaterialCoefficientMixingType.TakeMinimum:
                        staticFriction = Math.Min(body1.Material.staticFriction, body2.Material.staticFriction);
                        dynamicFriction = Math.Min(body1.Material.kineticFriction, body2.Material.kineticFriction);
                        restitution = Math.Min(body1.Material.restitution, body2.Material.restitution);
                        break;
                    case ContactSettings.MaterialCoefficientMixingType.UseAverage:
                        staticFriction = (body1.Material.staticFriction + body2.Material.staticFriction) / 2.0f;
                        dynamicFriction = (body1.Material.kineticFriction + body2.Material.kineticFriction) / 2.0f;
                        restitution = (body1.Material.restitution + body2.Material.restitution) / 2.0f;
                        break;
                }

            }

            this.settings = settings;



        }
    }
    */
}