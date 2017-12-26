using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics
{
    public struct Sphere3
    {
        public Vector3 Position;
        public float Radius;

        public float X { get { return Position.X; } set { Position.X = value; } }
        public float Y { get { return Position.Y; } set { Position.Y = value; } }
        public float Z { get { return Position.Z; } set { Position.Z = value; } }

        public Sphere3(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public bool Contains(Vector3 vector)
        {
            return Vector3.Distance(ref Position, ref vector) < Radius;
        }

        public bool Intersects(Sphere3 sphere)
        {
            var distanceSquared = Vector3.DistanceSquared(ref Position, ref sphere.Position);
            var jointRadius = (Radius + sphere.Radius);
            return distanceSquared < jointRadius;
        }

        public bool Intersects(Bounds3 bounds)
        {
            Functions.Clamp(ref Position, ref bounds.Minimum, ref bounds.Maximum, out var closest);
            var distance = Vector3.Distance(Position, closest);
            return distance < Radius;
        }
    }
}
