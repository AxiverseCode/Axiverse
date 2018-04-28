using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    using Vector2 = SharpDX.Vector2;
    using Vector3 = SharpDX.Vector3;
    using Vector4 = SharpDX.Vector4;

    public class ObjMesh : Mesh<PositionColorTexture>
    {
        public ObjMesh(Renderer renderer) : base(renderer)
        {

        }

        public void Load(string path)
        {
            var device = Renderer.Device;

            var loader = new WavefrontObjLoader();
            loader.LoadObj(path);

            // vertex buffer
            VertexCount = loader.TriangleCount * 3;
            var vertices = new PositionColorTexture[VertexCount];

            int i = 0;
            foreach (var face in loader.FaceList)
            {
                var v = face.VertexIndexList;
                var t = face.TextureVertexIndexList;

                vertices[i + 0] = new PositionColorTexture { Position = loader.VertexList[v[0] - 1].Vector, Color = new Vector4(1, 1, 1, 1), Texture = loader.TextureList[t[0] - 1].Vector };
                vertices[i + 2] = new PositionColorTexture { Position = loader.VertexList[v[1] - 1].Vector, Color = new Vector4(1, 1, 1, 1), Texture = loader.TextureList[t[1] - 1].Vector };
                vertices[i + 1] = new PositionColorTexture { Position = loader.VertexList[v[2] - 1].Vector, Color = new Vector4(1, 1, 1, 1), Texture = loader.TextureList[t[2] - 1].Vector };

                i += 3;

                if (v.Length == 4)
                {
                    vertices[i + 0] = new PositionColorTexture { Position = loader.VertexList[v[2] - 1].Vector, Color = new Vector4(1, 1, 1, 1), Texture = loader.TextureList[t[2] - 1].Vector };
                    vertices[i + 2] = new PositionColorTexture { Position = loader.VertexList[v[3] - 1].Vector, Color = new Vector4(1, 1, 1, 1), Texture = loader.TextureList[t[3] - 1].Vector };
                    vertices[i + 1] = new PositionColorTexture { Position = loader.VertexList[v[0] - 1].Vector, Color = new Vector4(1, 1, 1, 1), Texture = loader.TextureList[t[0] - 1].Vector };

                    i += 3;
                }
            }

            var vertexBufferSize = Utilities.SizeOf(vertices);
            // var vertexUploadBuffer = device.CreateCommittedResource(new HeapProperties(HeapType.Upload), HeapFlags.None, ResourceDescription.Buffer(vertexBufferSize), ResourceStates.GenericRead);
            VertexBufferResource = device.CreateCommittedResource(
                new HeapProperties(HeapType.Upload),
                HeapFlags.None,
                ResourceDescription.Buffer(vertexBufferSize),
                ResourceStates.GenericRead);

            // upload
            IntPtr vertexData = VertexBufferResource.Map(0);
            Utilities.Write(vertexData, vertices, 0, vertices.Length);
            VertexBufferResource.Unmap(0);

            // vertex buffer view
            VertexBufferView = new VertexBufferView()
            {
                BufferLocation = VertexBufferResource.GPUVirtualAddress,
                StrideInBytes = Utilities.SizeOf<PositionColorTexture>(),
                SizeInBytes = vertexBufferSize,
            };
        }
    }
}
