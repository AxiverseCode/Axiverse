using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Entites
{
    public class Camera : Component
    {
        public Matrix4 View;
        public Matrix4 Projection;
        public Vector3 Position;
    }
}
