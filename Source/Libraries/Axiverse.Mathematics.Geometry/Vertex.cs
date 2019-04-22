using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    /// <summary>
    /// Represents the mesh vertex.
    /// </summary>
    public struct Vertex
    {
        /// <summary>
        /// Gets or sets the position of the vertex.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Gets or sets the normal of the vertex.
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// Gets or sets the tangent of the vertex. This points to the U axis on the normal map.
        /// </summary>
        public Vector3 Tangent;

        /// <summary>
        /// Gets or sets the binormal of the vertex. This points to the V axis on the normal map.
        /// </summary>
        public Vector3 Binormal;

        /// <summary>
        /// Gets or sets the texture coordinates of the vertex.
        /// </summary>
        public Vector2 Texture;

        /// <summary>
        /// Gets or sets the color of the vertex.
        /// </summary>
        public Vector4 Color;

        /// <summary>
        /// Constructs a vertex.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="normal"></param>
        /// <param name="tangent"></param>
        /// <param name="binormal"></param>
        /// <param name="texture"></param>
        /// <param name="color"></param>
        public Vertex(Vector3 position = default,
            Vector3 normal = default,
            Vector3 tangent = default,
            Vector3 binormal = default, 
            Vector2 texture = default,
            Vector4 color = default)
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            Binormal = binormal;
            Texture = texture;
            Color = color;
        }

        public override string ToString()
        {
            return Tangent.ToString();
        }
    }
}
