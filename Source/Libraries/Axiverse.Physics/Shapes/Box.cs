using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;

namespace Axiverse.Physics.Shapes
{
    /// <summary>
    /// A oriented box collision shape.
    /// </summary>
    public class Box : Shape
    {
        public Matrix3 transform;
        public Vector3 size;
        public Vector3 halfSize;

        public Box(Vector3 size)
        {

        }

        public override Bounds3 CalculateBounds(Matrix4 transform)
        {
            throw new NotImplementedException();
        }

        public Bounds3 GetBoundingBox(ref Matrix3 orientation, out Bounds3 bounds)
        {
            throw new NotFiniteNumberException();
        }

        public void CalculateMassInertia()
        {
            //Mass = size.X * size.Y * size.Z;

            //Inertia = Matrix3.Identity;
            //Inertia.M11 = (1.0f / 12.0f) * Mass * (size.Y * size.Y + size.Z * size.Z);
            //Inertia.M22 = (1.0f / 12.0f) * Mass * (size.X * size.X + size.Z * size.Z);
            //Inertia.M33 = (1.0f / 12.0f) * Mass * (size.X * size.X + size.Y * size.Y);

            //Origin = Vector3.Zero;
        }

        public Vector3 FurthestPoint(Vector3 direction)
        {
            Vector3 result;
            result.X = (float)Math.Sign(direction.X) * halfSize.X;
            result.Y = (float)Math.Sign(direction.Y) * halfSize.Y;
            result.Z = (float)Math.Sign(direction.Z) * halfSize.Z;
            return result;
        }
    }
    
}
