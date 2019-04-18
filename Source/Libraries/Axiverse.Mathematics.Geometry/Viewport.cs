using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    public class Viewport
    {
        public Bounds3 Bounds;

        public float X => Bounds.Minimum.X;
        public float Y => Bounds.Minimum.Y;
        public float Width => Bounds.Minimum.X;
        public float Height => Bounds.Minimum.Y;

        public Viewport(float x, float y, float width, float height, float minZ = 0, float maxZ = 1)
        {
            Bounds = new Bounds3(x, y, minZ, x + width, y + height, maxZ);
        }

        public Vector2 FromClipSpace(Vector2 clipPoint)
        {
            // [-1, 1]
            return (clipPoint + Vector2.One) * Width / 2 + new Vector2(X, Y);
        }

        public Vector2 ToClipSpace(Vector2 screenPoint)
        {
            return (screenPoint - new Vector2(X, Y)) / Width * 2 - Vector2.One;
        }
    }
}
