using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    /// <summary>
    /// Mesh.
    /// </summary>
    /// <remarks>
    /// http://www.gradientspace.com/tutorials/dmesh3
    /// </remarks>
    public class Mesh
    {
        /// <summary>
        /// Gets a list of quadrilateral faces of the mesh.
        /// </summary>
        public List<Index4> Quadrilaterals { get; } = new List<Index4>();

        /// <summary>
        /// Gets a list of triangle faces of the mesh.
        /// </summary>
        public List<Index3> Triangles { get; } = new List<Index3>();

        /// <summary>
        /// Gets a list of vertices of the mesh.
        /// </summary>
        public List<Vertex> Vertices { get; } = new List<Vertex>();

        /// <summary>
        /// Calculates normals based on texture vertices.
        /// </summary>
        public void CalculateNormals()
        {
            // https://stackoverflow.com/questions/5255806/how-to-calculate-tangent-and-binormal
            foreach (var triangle in Triangles)
            {
                Vector3 deltaPos1 = Vertices[triangle.B].Position - Vertices[triangle.A].Position;
                Vector3 deltaPos2 = Vertices[triangle.C].Position - Vertices[triangle.A].Position;

                Vector2 deltaTex1 = Vertices[triangle.B].Texture - Vertices[triangle.A].Texture;
                Vector2 deltaTex2 = Vertices[triangle.C].Texture - Vertices[triangle.A].Texture;

                float r = 1.0f / (deltaTex1.X * deltaTex2.Y - deltaTex1.Y * deltaTex2.X);
                Vector3 tangent = (deltaPos1 * deltaTex2.Y - deltaPos2 * deltaTex1.Y) * r;
                Vector3 binormal = (deltaPos2 * deltaTex1.X - deltaPos1 * deltaTex2.X) * r;

                Vertex vertex;

                vertex = Vertices[triangle.A];
                vertex.Tangent = tangent;
                vertex.Binormal = binormal;
                Vertices[triangle.A] = vertex;

                vertex = Vertices[triangle.B];
                vertex.Tangent = tangent;
                vertex.Binormal = binormal;
                Vertices[triangle.B] = vertex;

                vertex = Vertices[triangle.C];
                vertex.Tangent = tangent;
                vertex.Binormal = binormal;
                Vertices[triangle.C] = vertex;
            }
        }

        public void ApplyTransform(Matrix4 transformation)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var vertex = Vertices[i];
                Matrix4.Transform(ref vertex.Position, ref transformation, out vertex.Position);
                Vertices[i] = vertex;
            }
        }

        /// <summary>
        /// Converts all quadrilaterals into triangles.
        /// </summary>
        public void Triangulate()
        {

        }
    }
}