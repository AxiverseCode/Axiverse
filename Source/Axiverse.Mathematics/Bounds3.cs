using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    public struct Bounds3
    {
        public Vector3 Minimum;
        public Vector3 Maximum;

        public Bounds3(Vector3 minimum, Vector3 maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public Bounds3(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            Minimum = new Vector3(minX, minY, minZ);
            Maximum = new Vector3(maxX, maxY, maxZ);
        }

        public bool Contains(Vector3 vector)
        {
            return (vector.X >= Minimum.X && vector.Y >= Minimum.Y && vector.Z >= Minimum.Z)
                && (vector.X <= Maximum.X && vector.Y <= Maximum.Y && vector.Z <= Maximum.Z);
        }

        public bool Intersects(Bounds3 bounds)
        {
            return (Minimum.X <= bounds.Maximum.X && Minimum.Y <= bounds.Maximum.Y && Minimum.Z <= bounds.Maximum.Z)
                && (Maximum.X >= bounds.Minimum.X && Maximum.Y >= bounds.Minimum.Y && Maximum.Z >= bounds.Minimum.Z);
        }

        public bool Intersects(Sphere3 sphere)
        {
            return sphere.Intersects(this);
        }

        public static bool Intersects(ref Bounds3 left, ref Bounds3 right)
        {
            return left.Intersects(right);
        }
    }
}
