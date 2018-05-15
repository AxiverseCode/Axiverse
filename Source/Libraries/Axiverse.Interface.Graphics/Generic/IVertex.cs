using System;
using System.Collections.Generic;
    using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Graphics.Generic
{
    public interface IVertex
    {
        VertexLayout Layout { get; }

        bool HasPosition { get; }
        Vector3 Position { get; set; }

        bool HasNormal { get; }
        Vector3 Normal { get; set; }

        bool HasTexture { get; }
        Vector2 Texture { get; set; }
    }

    /// <summary>
    /// Represents a vertex with position, color, and texture fields
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PositionNormalColorTexture : IVertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 Texture;
        public Vector4 Color;

        public static readonly VertexLayout Layout = new VertexLayout(
            new VertexElement("Position", 0, VertexFormat.Vector3),
            new VertexElement("Normal", 12, VertexFormat.Vector3),
            new VertexElement("Texture", 24, VertexFormat.Vector2),
            new VertexElement("Color", 32, VertexFormat.Vector4));

        VertexLayout IVertex.Layout => Layout;

        bool IVertex.HasPosition => true;
        Vector3 IVertex.Position { get => Position; set => Position = value; }

        bool IVertex.HasNormal => true;
        Vector3 IVertex.Normal { get => Normal; set => Normal = value; }

        bool IVertex.HasTexture => true;
        Vector2 IVertex.Texture { get => Texture; set => Texture = value; }
    }

    /// <summary>
    /// Represents a vertex with position, color, and texture fields
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PositionColorTexture : IVertex
    {
        public Vector3 Position;
        public Vector2 Texture;
        public Vector4 Color;
        
        public static readonly VertexLayout Layout = new VertexLayout(
            new VertexElement("Position", 0, VertexFormat.Vector3),
            new VertexElement("Texture", 12, VertexFormat.Vector2),
            new VertexElement("Color", 20, VertexFormat.Vector4));

        VertexLayout IVertex.Layout => Layout;

        bool IVertex.HasPosition => true;
        Vector3 IVertex.Position { get => Position; set => Position = value; }

        bool IVertex.HasNormal => false;
        Vector3 IVertex.Normal { get => throw new NotImplementedException(); set { } }

        bool IVertex.HasTexture => true;
        Vector2 IVertex.Texture { get => Texture; set => Texture = value; }

        public PositionColorTexture(Vector3 position, Vector2 texture, Vector4 color)
        {
            Position = position;
            Texture = texture;
            Color = color;
        }

        public PositionColorTexture(float x, float y, float z, float u, float v, float r, float g, float b, float a)
        {
            Position = new Vector3(x, y, z);
            Texture = new Vector2(u, v);
            Color = new Vector4(r, g, b, a);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PositionColorTextureMaterial : IVertex
    {
        public Vector3 Position;
        public Vector2 Texture;
        public Vector4 Color;
        public uint MaterialId;

        public static readonly VertexLayout Layout = new VertexLayout(
            new VertexElement("Position", 0, VertexFormat.Vector3),
            new VertexElement("Texture", 12, VertexFormat.Vector2),
            new VertexElement("Color", 20, VertexFormat.Vector4),
            new VertexElement("MaterialId", 36, VertexFormat.Vector2));

        VertexLayout IVertex.Layout => Layout;

        bool IVertex.HasPosition => true;
        Vector3 IVertex.Position { get => Position; set => Position = value; }

        bool IVertex.HasNormal => false;
        Vector3 IVertex.Normal { get => throw new NotImplementedException(); set { } }

        bool IVertex.HasTexture => true;
        Vector2 IVertex.Texture { get => Texture; set => Texture = value; }
    }
}
