using System;
using System.Text;
using System.Collections.Generic;

using Axiverse.Mathematics;
using Axiverse.Mathematics.Spatial;

namespace Axiverse.Mathematics.Spatial
{
    public class Spatial3 : ISpatial3
    {
        public Vector3 Position
        {
            get => sphere.Position;
            set
            {
                sphere.Position = value;
                PositionChanged(this, null);
            }
        }

        public float Radius
        {
            get => sphere.Radius;
            set
            {
                sphere.Radius = value;
                RadiusChanged(this, null);
            }
        }

        public Sphere3 BoundingSphere
        {
            get => sphere;
            set
            {
                sphere = value;
                PositionChanged(this, null);
                RadiusChanged(this, null);
            }
        }

        public event EventHandler PositionChanged;
        public event EventHandler RadiusChanged;

        public Spatial3(Vector3 position, float radius)
        {
            sphere = new Sphere3(position, radius);
        }

        private Sphere3 sphere;
    }
}
