using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Entities
{
    public class Transform : Component
    {
        public Transform Parent { get; set; }
        public List<Transform> Children { get; } = new List<Transform>();
        public Vector3 Origin { get; private set; }

        public Matrix4 Composite;
        public Matrix4 Transformation;

        public Vector3 Scaling = Vector3.One;
        public Matrix3 Rotation = Matrix3.Identity;
        public Vector3 Translation;

        public void ComputeTransforms()
        {
            Matrix4.Transformation(ref Scaling, ref Rotation, ref Translation, out Transformation);
            Composite = (Parent == null) ? Transformation : Transformation * Parent.Composite;
            Origin = Matrix4.Transform(default(Vector3), Composite);

            foreach (var child in Children)
            {
                child.ComputeTransforms();
            }
        }
    }
}
