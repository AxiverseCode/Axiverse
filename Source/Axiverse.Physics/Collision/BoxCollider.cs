using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;
using Axiverse.Simulation.Physics.Shapes;

namespace Axiverse.Simulation.Physics.Collision
{
    /// <summary>
    /// 
    /// </summary>
    public class BoxCollider : Collider<Box, Box>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="former"></param>
        /// <param name="latter"></param>
        /// <returns></returns>
        public override Manifold Collide(Box former, Box latter)
        {
            // https://gamedev.stackexchange.com/questions/112883/simple-3d-obb-collision-directx9-c
            // https://github.com/RandyGaul/qu3e/blob/master/src/collision/q3Collide.cpp

            Matrix3 formerTransform = former.transform;
            Matrix3 latterTransform = latter.transform;

            Vector3 formerSize = former.size;
            Vector3 latterSize = latter.size;

            // latter's frame in former space
            Matrix3 C = Matrix3.Transpose(formerTransform) * latterTransform;

            Matrix3 absoluteC = Matrix3.Zero;
            bool parallel = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    float value = Math.Abs(C[i,j]);
                    if (value + float.Epsilon > 1)
                    {
                        parallel = true;
                    }
                }
            }

            // vector from the center of former to the center of latter in former space
            Vector3 difference = Matrix3.Transform(formerTransform, latter.Position - former.Position);

            // query states
            float s;
            float aMax = float.NegativeInfinity;
            float bMax = float.NegativeInfinity;
            float eMax = float.NegativeInfinity;
            int aAxis = ~0;
            int bAxis = ~0;
            int eAxis = ~0;
            Vector3 nA = Vector3.Zero;
            Vector3 nB = Vector3.Zero;
            Vector3 nC = Vector3.Zero;

            // former's X axis
            s = Math.Abs(difference.X) - (formerSize.X + Vector3.Dot(absoluteC.Column(0), latterSize));
            if (TrackFaceAxis(ref aAxis, 0, s,ref  aMax, formerTransform.Row(0), ref nA))
                return null;

            // former's Y axis
            s = Math.Abs(difference.Y) - (formerSize.Y + Vector3.Dot(absoluteC.Column(0), latterSize));
            if (TrackFaceAxis(ref aAxis, 1, s, ref aMax, formerTransform.Row(1), ref nA))
                return null;

            // former's Z axis
            s = Math.Abs(difference.Z) - (formerSize.Z + Vector3.Dot(absoluteC.Column(0), latterSize));
            if (TrackFaceAxis(ref aAxis, 2, s, ref aMax, formerTransform.Row(2), ref nA))
                return null;

            // latter's X axis
            s = Math.Abs(Vector3.Dot(difference, C.Row(0))) - (latterSize.X + Vector3.Dot(absoluteC.Row(0), formerSize));
            if (TrackFaceAxis(ref bAxis, 3, s, ref bMax, latterTransform.Row(0), ref nB))
                return null;

            // latter's Y axis
            s = Math.Abs(Vector3.Dot(difference, C.Row(1))) - (latterSize.Y + Vector3.Dot(absoluteC.Row(1), formerSize));
            if (TrackFaceAxis(ref bAxis, 4, s, ref bMax, latterTransform.Row(1), ref nB))
                return null;

            // latter's Z axis
            s = Math.Abs(Vector3.Dot(difference, C.Row(2))) - (latterSize.Z + Vector3.Dot(absoluteC.Row(2), formerSize));
            if (TrackFaceAxis(ref bAxis, 5, s, ref bMax, latterTransform.Row(2), ref nB))
                return null;

            if (!parallel)
            {

            }

            throw new NotImplementedException();
        }

        protected bool TrackFaceAxis(ref int axis, int n, float s, ref float sMax, Vector3 normal, ref Vector3 axisNormal)
        {
            if (s > 0)
            {
                return true;
            }

            if (s > sMax)
            {
                sMax = s;
                axis = n;
                axisNormal = normal;
            }
            return false;
        }
    }
}
