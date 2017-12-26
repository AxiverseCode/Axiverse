using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace Axiverse.Interface
{
    public class TransformComponent
    {
        public Matrix Transformation { get; set; }

        public TransformComponent()
        {
            Transformation = Matrix.Identity;
        }
    }
}
