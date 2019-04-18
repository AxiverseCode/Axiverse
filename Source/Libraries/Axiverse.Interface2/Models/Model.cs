using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer11 = SharpDX.Direct3D11.Buffer;

namespace Axiverse.Interface2.Models
{
    using static Axiverse.Interface2.Mesh;
    using Mesh = Axiverse.Mathematics.Geometry.Mesh;

    public class Model : IDisposable
    {
        public Device Device { get; set; }
        public Buffer11 VertexBuffer { get; set; }
        public Buffer11 IndexBuffer { get; set; }
        public InputElement[] InputLayout { get; set; }
        public int Stride { get; set; }

        public List<Material> Materials { get; } = new List<Material>();
        public List<Segment> Segments { get; } = new List<Segment>();

        public Model(Device device)
        {
            Device = device;
        }

        public void Draw(Shader shader)
        {
            shader.Apply(InputLayout);

            Device.NativeDeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            Device.NativeDeviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, Stride, 0));
            Device.NativeDeviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);

            foreach (var segment in Segments)
            {
                shader.Prepare(this, segment);
                Device.NativeDeviceContext.DrawIndexed(segment.Count, segment.Offset, 0);
            }
        }

        public static Model FromMesh(Device device, Mesh mesh)
        {
            var value = new Model(device);
            value.InputLayout = Vertex.Elements;
            value.Stride = Vertex.Stride;

            mesh.Triangulate();
            var vertices = new Vertex[mesh.Vertices.Count];

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                var v = mesh.Vertices[i];
                vertices[i] = new Vertex(
                    v.Position,
                    normal: v.Normal,
                    tangent: v.Tangent,
                    binormal: v.Binormal,
                    color: v.Color,
                    texture: v.Texture);
            }

            var indices = new int[mesh.Triangles.Count * 3];
            for (int i = 0; i < mesh.Triangles.Count; i++)
            {
                indices[i * 3 + 0] = mesh.Triangles[i].A;
                indices[i * 3 + 1] = mesh.Triangles[i].B;
                indices[i * 3 + 2] = mesh.Triangles[i].C;
            }

            value.VertexBuffer = Buffer11.Create<Vertex>(device.NativeDevice, BindFlags.VertexBuffer, vertices);
            value.IndexBuffer = Buffer11.Create(device.NativeDevice, BindFlags.IndexBuffer, indices.ToArray());
            value.Segments.Add(new Segment
            {
                Count = mesh.Triangles.Count
            });

            return value;
        }

        public void Dispose()
        {
            VertexBuffer?.Dispose();
            VertexBuffer = null;
            IndexBuffer?.Dispose();
            IndexBuffer = null;
        }

        public class Segment
        {
            public int Offset;
            public int Count;
            public int Material;
        }

        public struct Vertex
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector3 Tangent;
            public Vector3 Binormal;
            public Vector2 Texture;
            public Vector4 Color;

            public Vertex(
                Vector3 position,
                Vector3 normal = default,
                Vector3 tangent = default,
                Vector3 binormal = default,
                Vector4 color = default,
                Vector2 texture = default)
            {
                Position = position;
                Normal = normal;
                Tangent = tangent;
                Binormal = binormal;
                Color = color;
                Texture = texture;
            }

            public static readonly int Stride = Utilities.SizeOf<Vertex>();
            public static readonly InputElement[] Elements = new InputElement[] {
                    new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                    new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                    new InputElement("TANGENT", 0, Format.R32G32B32_Float, 24, 0),
                    new InputElement("BINORMAL", 0, Format.R32G32B32_Float, 36, 0),
                    new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 48, 0),
                    new InputElement("TEXCOORD", 0, Format.R32G32_Float, 64, 0)
                };
        }
    }
}
