using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Implementing GJK (2006) - Casey Muratori
    /// https://youtu.be/Qupqu1xe7Io, https://caseymuratori.com/blog_0003
    /// 
    /// GJK (Gilbert–Johnson–Keerthi) - William Bittle
    /// http://www.dyn4j.org/2010/04/gjk-gilbert-johnson-keerthi/
    /// 
    /// GJK Algorithm 3D - Sergiu Caritoiu
    /// http://in2gpu.com/2014/05/18/gjk-algorithm-3d/
    /// 
    /// Gilbert–Johnson–Keerthi (GJK) 3D distance algorithm - Micha Mettke
    /// https://gist.github.com/vurtun/29727217c269a2fbf4c0ed9a1d11cb40
    /// </remarks>
    public class GilbertJohnsonKeerthi
    {
        private const int Iterations = 50;

        Vector3 A;
        Vector3 B;
        Vector3 C;
        Vector3 D;

        int simplexCount;

        public bool Collide(IEnumerable<Vector3> former, IEnumerable<Vector3> latter)
        {
            Vector3 direction = Vector3.One;

            C = Support(former, latter, ref direction);

            direction = -C;

            B = Support(former, latter, ref direction);

            if (B.Dot(direction) < 0)
            {
                return false;
            }

            direction = Vector3.DoubleCross(C - B, -B);
            simplexCount = 2;

            for (int i = 0; i < Iterations; i++)
            {
                A = Support(former, latter, ref direction);
                if (A.Dot(direction) < 0)
                {
                    return false;
                }
                else
                {
                    if (ContainsOrigin(ref direction))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ContainsOrigin(ref Vector3 direction)
        {
            switch (simplexCount)
            {
                case 1:
                    return Line(ref direction);
                case 2:
                    return Triangle(ref direction);
                case 3:
                    return Tetrahedron(ref direction);
                default:
                    return false;
            }
        }

        private bool Line(ref Vector3 direction)
        {
            // Point can't be behind B.
            Vector3 ab = B - A;
            Vector3 ao = /* 0 */ -A;

            // New direction towards a0.
            Vector3.DoubleCross(ref ab, ref ao, out direction);

            C = B;
            B = A;
            simplexCount = 2;
            
            return false;
        }

        private bool Triangle(ref Vector3 direction)
        {
            // Point can't be behind or is in the direction of B, C, or BC
            Vector3 ao = -A;
            Vector3 ab = B - A;
            Vector3 ac = C - A;
            Vector3 abc = Vector3.Cross(ab, ac);

            Vector3 ab_abc = Vector3.Cross(ab, abc);

            if (ab_abc.Dot(ao) > 0)
            {
                C = B;
                B = A;

                Vector3.DoubleCross(ref ab, ref ao, out direction);

                // Change direction, can't build tetrahedron.
                return false;
            }

            if (abc.Dot(ao) > 0)
            {
                // Base of tetrahedron.
                D = C;
                C = B;
                B = A;
                direction = abc;
            }
            else
            {
                // Upside down tetrahedron.
                D = B;
                B = A;
                direction = -abc;
            }

            simplexCount = 3;
            return false;
        }

        private bool Tetrahedron(ref Vector3 direction)
        {
            Vector3 ao = -A;
            Vector3 ab = B - A;
            Vector3 ac = C - A;
            Vector3 abc = Vector3.Cross(ab, ac);

            // Case 1: In front of triangle.
            if (abc.Dot(ao) > 0)
            {
                // In front of triangle ABC.
                return CheckTetrahedron(ref ao, ref ab, ref ac, ref abc, ref direction);
            }

            // Case 2: Same direction as AO
            Vector3 ad = D - A;
            Vector3 acd = Vector3.Cross(ac, ad);
            if (acd.Dot(ao) > 0)
            {
                // In front of triangle ACD.
                B = C;
                C = D;
                ab = ac;
                ac = ad;
                abc = acd;
                return CheckTetrahedron(ref ao, ref ab, ref ac, ref abc, ref direction);
            }

            Vector3 adb = Vector3.Cross(ad, ab);
            if (adb.Dot(ao) > 0)
            {
                // In front of triangle ADB.
                C = B;
                B = D;
                ac = ab;
                ab = ad;
                abc = adb;
                return CheckTetrahedron(ref ao, ref ab, ref ac, ref abc, ref direction);
            }

            // Origin is inside tetrahedron.
            return true;
        }

        private bool CheckTetrahedron(ref Vector3 ao, ref Vector3 ab, ref Vector3 ac, ref Vector3 abc, ref Vector3 direction)
        {
            Vector3 ab_abc = Vector3.Cross(ab, abc);

            if (ab_abc.Dot(ao) > 0)
            {
                C = B;
                B = A;

                // Direction is not ab_abc because it does not point towards the origin.
                // AB x A0 x AB is the direction that we want.
                direction = Vector3.DoubleCross(ab, ao);

                // Build a new triangle and discard D.
                simplexCount = 2;
                return false;
            }

            Vector3 acp = Vector3.Cross(abc, ac);

            if (acp.Dot(ao) > 0)
            {
                B = A;

                // Direction is not acp because it does not point towards the origin.
                // AC x A0 x AC is the direction that we want.
                direction = Vector3.DoubleCross(ac, ao);

                // Build a new triangle and discard D.
                simplexCount = 2;
                return false;
            }

            // Build a new tetrahedron with a new base.
            D = C;
            C = B;
            B = A;
            direction = abc;

            simplexCount = 3;
            return false;
        }

        public Vector3 Support(IEnumerable<Vector3> a, IEnumerable<Vector3> b, ref Vector3 direction)
        {
            Vector3 formerPoint = Vector3.Zero; // getFurthestPointInDirection(dir)
            Vector3 latterPoint = Vector3.Zero; // getFutherstPointInDirection(-dir)

            Vector3 minkowskiDifference = formerPoint - latterPoint;
            return minkowskiDifference;
        }
    }
}
