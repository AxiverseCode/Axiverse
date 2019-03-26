using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    /// <summary>
    /// Collisions
    /// </summary>
    /// <remarks>
    /// http://www.realtimerendering.com/intersections.html
    /// </remarks>
    public static class Collision
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ra"></param>
        /// <param name="A0"></param>
        /// <param name="A1"></param>
        /// <param name="rb"></param>
        /// <param name="B0"></param>
        /// <param name="B1"></param>
        /// <param name="u0"></param>
        /// <param name="u1"></param>
        /// <returns></returns>
        public static bool SweptSphereSphere(float ra, Vector3 A0, Vector3 A1, float rb, Vector3 B0, Vector3 B1, out float u0, out float u1)
        {
            // http://www.gamasutra.com/view/feature/131790/simple_intersection_tests_for_games.php?page=2

            var va = A1 - A0;
            // vector from A0 to A1

            var vb = B1 - B0;
            // vector from B0 to B1

            var AB = B0 - A0;
            // vector from A0 to B0

            var vab = vb - va;
            // relative velocity (in normalized time)

            float rab = ra + rb;

            float a = vab.Dot(vab);
            // u*u coefficient

            float b = 2 * vab.Dot(AB);
            // u coefficient

            float c = AB.Dot(AB) - rab * rab;

            // constant term
            // check if they're currently overlapping
            if (AB.Dot(AB) <= rab * rab)

            {
                u0 = 0;
                u1 = 0;
                return true;
            }

            // check if they hit each other
            // during the frame
            if (Polynomial.QuadraticFormula(a, b, c, out var v0, out var v1))
            {
                u0 = Math.Min(v0, v1);
                u1 = Math.Max(v0, v1);
                return true;
            }

            u0 = u1 = float.NaN;
            return false;
        }
    }
}
