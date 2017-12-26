using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Mathematics;

namespace Axiverse.Simulation.Physics.Shapes
{
    public abstract class Shape
    {
        internal float Mass;
        internal Matrix3 Inertia;
        internal Vector3 Origin;
        public Vector3 Position;

        public void ComputeBoundingBox()
        {

        }

        //public abstract Vector3 FurthestPoint(Vector3 axis);
        //public abstract Vector3 Center();


    }
}
