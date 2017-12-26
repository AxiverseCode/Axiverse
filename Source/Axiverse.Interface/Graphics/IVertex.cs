using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D12;
using SharpDX.DXGI;

namespace Axiverse.Interface.Graphics
{
    using Vector2 = SharpDX.Vector2;
    using Vector3 = SharpDX.Vector3;
    using Vector4 = SharpDX.Vector4;

    public interface IVertex
    {
        InputLayoutDescription Description { get; }

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

        public static readonly InputLayoutDescription Description = new InputLayoutDescription(new InputElement[]
            {
                new InputElement("Position", 0, Format.R32G32B32_Float, 0, 0),
                new InputElement("Normal", 0, Format.R32G32B32_Float, 12, 0),
                new InputElement("Texture", 0, Format.R32G32_Float, 24, 0),
                new InputElement("Color", 0, Format.R32G32B32A32_Float, 32, 0),
            });

        InputLayoutDescription IVertex.Description => Description;

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
        
        public static readonly InputLayoutDescription Description = new InputLayoutDescription(new InputElement[]
            {
                new InputElement("Position", 0, Format.R32G32B32_Float, 0, 0),
                new InputElement("Texture", 0, Format.R32G32_Float, 12, 0),
                new InputElement("Color", 0, Format.R32G32B32A32_Float, 20, 0),
            });

        InputLayoutDescription IVertex.Description => Description;

        bool IVertex.HasPosition => true;
        Vector3 IVertex.Position { get => Position; set => Position = value; }

        bool IVertex.HasNormal => false;
        Vector3 IVertex.Normal { get => throw new NotImplementedException(); set { } }

        bool IVertex.HasTexture => true;
        Vector2 IVertex.Texture { get => Texture; set => Texture = value; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PositionColorTextureMaterial : IVertex
    {
        public Vector3 Position;
        public Vector2 Texture;
        public Vector4 Color;
        public uint MaterialId;

        public static readonly InputLayoutDescription Description = new InputLayoutDescription(new InputElement[]
            {
                new InputElement("Position", 0, Format.R32G32B32_Float, 0, 0),
                new InputElement("Texture", 0, Format.R32G32_Float, 12, 0),
                new InputElement("Color", 0, Format.R32G32B32A32_Float, 20, 0),
                new InputElement("MaterialId", 0, Format.R32_UInt, 36, 0),
            });

        InputLayoutDescription IVertex.Description => Description;

        bool IVertex.HasPosition => true;
        Vector3 IVertex.Position { get => Position; set => Position = value; }

        bool IVertex.HasNormal => false;
        Vector3 IVertex.Normal { get => throw new NotImplementedException(); set { } }

        bool IVertex.HasTexture => true;
        Vector2 IVertex.Texture { get => Texture; set => Texture = value; }
    }
}
