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

namespace Axiverse.Interface2
{
    public class Mesh
    {
        public Device Device { get; set; }
        public Buffer11 VertexBuffer { get; set; }
        public Buffer11 IndexBuffer { get; set; }
        public int VertexSize { get; set; }
        public int IndexCount { get; set; }

        /// <summary>
        /// Draw Mesh
        /// </summary>
        public void Draw()
        {
            Device.NativeDeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            Device.NativeDeviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, VertexSize, 0));
            Device.NativeDeviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);
            Device.NativeDeviceContext.DrawIndexed(IndexCount, 0, 0);
        }

        private Mesh(Device device)
        {
            Device = device;
        }

        /// <summary>
        /// Create a quad for Multiple Render Target
        /// </summary>
        /// <param name="device">Device</param>
        /// <returns>Mesh</returns>
        public static Mesh CreateQuad(Device device)
        {
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(-1, 1, 0),
                new Vector3(-1, -1, 0),
                new Vector3(1, 1, 0),
                new Vector3(1, -1, 0)
            };

            int[] indices = new int[] { 0, 2, 1, 2, 3, 1 };
            Mesh mesh = new Mesh(device);
            mesh.VertexBuffer = Buffer11.Create<Vector3>(device.NativeDevice, BindFlags.VertexBuffer, vertices.ToArray());
            mesh.IndexBuffer = Buffer11.Create(device.NativeDevice, BindFlags.IndexBuffer, indices.ToArray());
            mesh.VertexSize = Utilities.SizeOf<Vector3>();

            mesh.IndexCount = indices.Count();

            return mesh;
        }

        public struct ColoredTexturedVertex
        {
            public Vector3 Position;
            public Vector4 Color;
            public Vector2 Texture;

            public ColoredTexturedVertex(Vector3 position, Vector4 color, Vector2 texture)
            {
                Position = position;
                Color = color;
                Texture = texture;
            }

            public static readonly int Stride = Utilities.SizeOf<ColoredTexturedVertex>();
            public static readonly InputElement[] Elements = new InputElement[] {
                    new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                    new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 12, 0),
                    new InputElement("TEXCOORD", 0, Format.R32G32_Float, 28, 0)
                };
        }

        public static Mesh CreateCube(Device device)
        {
            //Indices
            int[] indices = new int[]
            {
                0,1,2,0,2,3,
                4,6,5,4,7,6,
                8,9,10,8,10,11,
                12,14,13,12,15,14,
                16,18,17,16,19,18,
                20,21,22,20,22,23
            };


            //Vertices
            ColoredTexturedVertex[] vertices = new[]
            {
                ////TOP
                new ColoredTexturedVertex(new Vector3(-5,5,5),new Vector4(0,1,0,0),new Vector2(1,1)),
                new ColoredTexturedVertex(new Vector3(5,5,5),new Vector4(0,1,0,0),new Vector2(0,1)),
                new ColoredTexturedVertex(new Vector3(5,5,-5),new Vector4(0,1,0,0),new Vector2(0,0)),
                new ColoredTexturedVertex(new Vector3(-5,5,-5),new Vector4(0,1,0,0),new Vector2(1,0)),
                //BOTTOM
                new ColoredTexturedVertex(new Vector3(-5,-5,5),new Vector4(1,0,1,1),new Vector2(1,1)),
                new ColoredTexturedVertex(new Vector3(5,-5,5),new Vector4(1,0,1,1),new Vector2(0,1)),
                new ColoredTexturedVertex(new Vector3(5,-5,-5),new Vector4(1,0,1,1),new Vector2(0,0)),
                new ColoredTexturedVertex(new Vector3(-5,-5,-5),new Vector4(1,0,1,1),new Vector2(1,0)),
                //LEFT
                new ColoredTexturedVertex(new Vector3(-5,-5,5),new Vector4(1,0,0,1),new Vector2(0,1)),
                new ColoredTexturedVertex(new Vector3(-5,5,5),new Vector4(1,0,0,1),new Vector2(0,0)),
                new ColoredTexturedVertex(new Vector3(-5,5,-5),new Vector4(1,0,0,1),new Vector2(1,0)),
                new ColoredTexturedVertex(new Vector3(-5,-5,-5),new Vector4(1,0,0,1),new Vector2(1,1)),
                //RIGHT
                new ColoredTexturedVertex(new Vector3(5,-5,5),new Vector4(1,1,0,1),new Vector2(1,1)),
                new ColoredTexturedVertex(new Vector3(5,5,5),new Vector4(1,1,0,1),new Vector2(1,0)),
                new ColoredTexturedVertex(new Vector3(5,5,-5),new Vector4(1,1,0,1),new Vector2(0,0)),
                new ColoredTexturedVertex(new Vector3(5,-5,-5),new Vector4(1,1,0,1),new Vector2(0,1)),
                //FRONT
                new ColoredTexturedVertex(new Vector3(-5,5,5),new Vector4(0,1,1,1),new Vector2(1,0)),
                new ColoredTexturedVertex(new Vector3(5,5,5),new Vector4(0,1,1,1),new Vector2(0,0)),
                new ColoredTexturedVertex(new Vector3(5,-5,5),new Vector4(0,1,1,1),new Vector2(0,1)),
                new ColoredTexturedVertex(new Vector3(-5,-5,5),new Vector4(0,1,1,1),new Vector2(1,1)),
                //BACK
                new ColoredTexturedVertex(new Vector3(-5,5,-5),new Vector4(0,0,1,1),new Vector2(0,0)),
                new ColoredTexturedVertex(new Vector3(5,5,-5),new Vector4(0,0,1,1),new Vector2(1,0)),
                new ColoredTexturedVertex(new Vector3(5,-5,-5),new Vector4(0,0,1,1),new Vector2(1,1)),
                new ColoredTexturedVertex(new Vector3(-5,-5,-5),new Vector4(0,0,1,1),new Vector2(0,1))
            };

            Mesh mesh = new Mesh(device);
            mesh.VertexBuffer = Buffer11.Create<ColoredTexturedVertex>(device.NativeDevice, BindFlags.VertexBuffer, vertices.ToArray());
            mesh.IndexBuffer = Buffer11.Create(device.NativeDevice, BindFlags.IndexBuffer, indices.ToArray());
            mesh.VertexSize = Utilities.SizeOf<ColoredTexturedVertex>();

            mesh.IndexCount = indices.Count();

            return mesh;
        }
    }
}
