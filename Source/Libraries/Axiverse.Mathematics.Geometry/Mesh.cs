using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// http://www.gradientspace.com/tutorials/dmesh3
    /// </remarks>
    public class Mesh
    {
        public List<Index4> Quadrilaterals { get; }

        public List<Index3> Triangles { get; }

        public List<Vector4> Colors { get; }

        public List<Vector2> Normals { get; }

        public List<Vector2> Textures { get; }

        public List<Vector3> Positions { get; }

        public Mesh()
        {
            Positions = new List<Vector3>();
            Textures = new List<Vector2>();
            Normals = new List<Vector2>();
            Colors = new List<Vector4>();

            Triangles = new List<Index3>();
            Quadrilaterals = new List<Index4>();
        }
    }
}