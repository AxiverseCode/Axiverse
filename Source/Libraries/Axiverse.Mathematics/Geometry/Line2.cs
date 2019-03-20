using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    /// <summary>
    /// A three dimensional ray.
    /// </summary>
    public struct Line2
    {
        /// <summary>
        /// The origin of the ray.
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// The direction vector of the ray.
        /// </summary>
        public Vector2 Direction;

        /// <summary>
        /// Constructs a line.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="origin"></param>
        public Line2(Vector2 direction, Vector2 origin = default(Vector2))
        {
            Direction = direction;
            Origin = origin;
        }

        public static Vector2 ClosestPoint(Line2 line, Vector2 vector)
        {
            Vector2 AP = vector - line.Origin;       //Vector from A to P   
            Vector2 AB = line.Direction - line.Origin;       //Vector from A to B  

            float magnitudeAB = AB.LengthSquared();     //Magnitude of AB vector (it's length squared)     
            float ABAPproduct = AP.Dot(AB);    //The DOT product of a_to_p and a_to_b     
            float distance = ABAPproduct / magnitudeAB; //The normalized "distance" from a to your closest point  

            if (distance < 0)     //Check if P projection is over vectorAB     
            {
                return line.Origin;

            }
            else if (distance > 1)
            {
                return line.Direction;
            }
            else
            {
                return line.Origin + AB * distance;
            }
        }
    }
}
