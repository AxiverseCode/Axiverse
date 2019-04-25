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
        public Mesh CalculateNormals()
        {
            // https://stackoverflow.com/questions/5255806/how-to-calculate-tangent-and-binormal
            foreach (var triangle in Triangles)
            {
                Vector3 deltaPos1 = Vertices[triangle.B].Position - Vertices[triangle.A].Position;
                Vector3 deltaPos2 = Vertices[triangle.C].Position - Vertices[triangle.A].Position;

                Vector2 deltaTex1 = Vertices[triangle.B].Texture - Vertices[triangle.A].Texture;
                Vector2 deltaTex2 = Vertices[triangle.C].Texture - Vertices[triangle.A].Texture;

                float r = -1.0f / (deltaTex1.X * deltaTex2.Y - deltaTex1.Y * deltaTex2.X);
                Vector3 tangent = (deltaPos1 * deltaTex2.Y - deltaPos2 * deltaTex1.Y) * r;
                Vector3 binormal = (deltaPos2 * deltaTex1.X - deltaPos1 * deltaTex2.X) * r;
                tangent.Normalize();
                binormal.Normalize();
                var normal = deltaPos1 % deltaPos2;

                if (float.IsNaN(tangent.X))
                {
                    binormal = Vertices[triangle.B].Normal % deltaPos1;
                    tangent = Vertices[triangle.B].Normal % binormal;
                }

                Vertex vertex;

                vertex = Vertices[triangle.A];
                vertex.Tangent = tangent;
                vertex.Binormal = binormal;
                vertex.Normal = (vertex.Normal == Vector3.Zero) ? normal : vertex.Normal;
                Vertices[triangle.A] = vertex;

                vertex = Vertices[triangle.B];
                vertex.Tangent = tangent;
                vertex.Binormal = binormal;
                vertex.Normal = (vertex.Normal == Vector3.Zero) ? normal : vertex.Normal;
                Vertices[triangle.B] = vertex;

                vertex = Vertices[triangle.C];
                vertex.Tangent = tangent;
                vertex.Binormal = binormal;
                vertex.Normal = (vertex.Normal == Vector3.Zero) ? normal : vertex.Normal;
                Vertices[triangle.C] = vertex;
            }

            return this;
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

        public Mesh Invert()
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var vertex = Vertices[i];
                vertex.Normal = -vertex.Normal;
                Vertices[i] = vertex;
            }

            for (int i = 0; i < Triangles.Count; i++)
            {
                var triangle = Triangles[i];
                triangle.C = Triangles[i].B;
                triangle.B = Triangles[i].C;
                Triangles[i] = triangle;
            }

            return this;
        }

        public void SetIndices(int[] indices)
        {
            for (int i = 0; i < indices.Length; i += 3)
            {
                Triangles.Add(new Index3()
                {
                    A = indices[i],
                    B = indices[i + 1],
                    C = indices[i + 2],
                });
            }
        }

        public static Mesh CreateCube(float side = 1f)
        {
            // Indices
            int[] indices = new int[]
            {
                0,1,2,0,2,3,
                4,6,5,4,7,6,
                8,9,10,8,10,11,
                12,14,13,12,15,14,
                16,18,17,16,19,18,
                20,21,22,20,22,23
            };

            var h = side / 2f;  

            // Vertices
            Vertex[] vertices = new[]
            {
                // TOP
                new Vertex(new Vector3(-h, h, h), color: new Vector4(0, 1, 0, 1), texture: new Vector2(1, 1)),
                new Vertex(new Vector3(h, h, h), color: new Vector4(0, 1, 0, 1), texture: new Vector2(0, 1)),
                new Vertex(new Vector3(h, h, -h), color: new Vector4(0, 1, 0, 1), texture: new Vector2(0, 0)),
                new Vertex(new Vector3(-h, h, -h), color: new Vector4(0, 1, 0, 1), texture: new Vector2(1, 0)), 
                // BOTTOM
                new Vertex(new Vector3(-h, -h, h), color: new Vector4(1, 0, 1, 1), texture: new Vector2(1, 1)),
                new Vertex(new Vector3(h, -h, h), color: new Vector4(1, 0, 1, 1), texture: new Vector2(0, 1)),
                new Vertex(new Vector3(h, -h, -h), color: new Vector4(1, 0, 1, 1), texture: new Vector2(0, 0)),
                new Vertex(new Vector3(-h, -h, -h), color: new Vector4(1, 0, 1, 1), texture: new Vector2(1, 0)), 
                // LEFT
                new Vertex(new Vector3(-h, -h, h), color: new Vector4(1, 0, 0, 1), texture: new Vector2(0, 1)),
                new Vertex(new Vector3(-h, h, h), color: new Vector4(1, 0, 0, 1), texture: new Vector2(0, 0)),
                new Vertex(new Vector3(-h, h, -h), color: new Vector4(1, 0, 0, 1), texture: new Vector2(1, 0)),
                new Vertex(new Vector3(-h, -h, -h), color: new Vector4(1, 0, 0, 1), texture: new Vector2(1, 1)), 
                // RIGHT
                new Vertex(new Vector3(h, -h, h), color: new Vector4(1, 1, 0, 1), texture: new Vector2(1, 1)),
                new Vertex(new Vector3(h, h, h), color: new Vector4(1, 1, 0, 1), texture: new Vector2(1, 0)),
                new Vertex(new Vector3(h, h, -h), color: new Vector4(1, 1, 0, 1), texture: new Vector2(0, 0)),
                new Vertex(new Vector3(h, -h, -h), color: new Vector4(1, 1, 0, 1), texture: new Vector2(0, 1)), 
                // FRONT
                new Vertex(new Vector3(-h, h, h), color: new Vector4(0, 1, 1, 1), texture: new Vector2(1, 0)),
                new Vertex(new Vector3(h, h, h), color: new Vector4(0, 1, 1, 1), texture: new Vector2(0, 0)),
                new Vertex(new Vector3(h, -h, h), color: new Vector4(0, 1, 1, 1), texture: new Vector2(0, 1)),
                new Vertex(new Vector3(-h, -h, h), color: new Vector4(0, 1, 1, 1), texture: new Vector2(1, 1)), 
                // BACK
                new Vertex(new Vector3(-h, h, -h), color: new Vector4(0, 0, 1, 1), texture: new Vector2(0, 0)),
                new Vertex(new Vector3(h, h, -h), color: new Vector4(0, 0, 1, 1), texture: new Vector2(1, 0)),
                new Vertex(new Vector3(h, -h, -h), color: new Vector4(0, 0, 1, 1), texture: new Vector2(1, 1)),
                new Vertex(new Vector3(-h, -h, -h), color: new Vector4(0, 0, 1, 1), texture: new Vector2(0, 1))
            };

            var mesh = new Mesh();
            mesh.Vertices.AddRange(vertices);
            mesh.SetIndices(indices);

            return mesh;
        }

        public static Mesh CreateSphere(float diameter = 1, int tessellation = 10)
        {
            if (tessellation < 3) throw new ArgumentOutOfRangeException("tessellation", "Must be >= 3");

            int verticalSegments = tessellation;
            int horizontalSegments = tessellation * 2;

            var vertices = new Vertex[(verticalSegments + 1) * (horizontalSegments + 1)];
            var indices = new int[(verticalSegments) * (horizontalSegments + 1) * 6];

            float radius = diameter / 2;

            int vertexCount = 0;
            // Create rings of vertices at progressively higher latitudes.
            for (int i = 0; i <= verticalSegments; i++)
            {
                float v = 1.0f - (float)i / verticalSegments;

                var latitude = (float)((i * Math.PI / verticalSegments) - Math.PI / 2.0);
                var dy = (float)Math.Sin(latitude);
                var dxz = (float)Math.Cos(latitude);

                // Create a single ring of vertices at this latitude.
                for (int j = 0; j <= horizontalSegments; j++)
                {
                    float u = (float)j / horizontalSegments;

                    var longitude = (float)(j * 2.0 * Math.PI / horizontalSegments);
                    var dx = (float)Math.Sin(longitude);
                    var dz = (float)Math.Cos(longitude);

                    dx *= dxz;
                    dz *= dxz;

                    var normal = new Vector3(dx, dy, dz);
                    var textureCoordinate = new Vector2(u, v);

                    vertices[vertexCount++] = new Vertex(normal * radius, normal: normal, color: Vector4.One, texture: textureCoordinate);
                }
            }

            // Fill the index buffer with triangles joining each pair of latitude rings.
            int stride = horizontalSegments + 1;

            int indexCount = 0;
            for (int i = 0; i < verticalSegments; i++)
            {
                for (int j = 0; j <= horizontalSegments; j++)
                {
                    int nextI = i + 1;
                    int nextJ = (j + 1) % stride;


                    indices[indexCount++] = (i * stride + j);
                    indices[indexCount++] = (i * stride + nextJ);
                    indices[indexCount++] = (nextI * stride + j);

                    indices[indexCount++] = (nextI * stride + j);
                    indices[indexCount++] = (i * stride + nextJ);
                    indices[indexCount++] = (nextI * stride + nextJ);
                }
            }

            Mesh mesh = new Mesh();
            mesh.Vertices.AddRange(vertices);
            mesh.SetIndices(indices);

            return mesh;
        }
    }
}