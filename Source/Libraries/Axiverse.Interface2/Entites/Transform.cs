using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Entites
{
    public class Transform : Component
    {
        public Transform Parent { get; set; }
        public List<Transform> Children { get; set; }

        public Matrix4 Composite;
        public Matrix4 Transformation;

        public Vector3 Scaling;
        public Matrix3 Rotation;
        public Vector3 Translation;

        public void ComputeTransforms()
        {
            Matrix4.Transformation(ref Scaling, ref Rotation, ref Translation, out Transformation);
            Composite = (Parent == null) ? Transformation : Transformation * Parent.Composite;

            foreach (var child in Children)
            {
                child.ComputeTransforms();
            }
        }
    }
}
