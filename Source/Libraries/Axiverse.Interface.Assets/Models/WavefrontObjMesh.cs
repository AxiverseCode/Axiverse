using Axiverse.Injection;
using Axiverse.Interface.Graphics;
using Axiverse.Interface.Graphics.Generic;
using Axiverse.Resources;
using System;

namespace Axiverse.Interface.Assets.Models
{
    public class WavefrontObjMesh
    {
        public int VertexCount { get; set; }


        public static VertexBufferBinding Load(GraphicsDevice device, String filename, Matrix3? positionTransform = null)
        {
            var library = Injector.Global.Resolve<Library>();
            var loader = new WavefrontObjLoader();
            using (var stream = library.OpenRead(filename))
            {
                loader.LoadObj(stream);
            }

            if (positionTransform.HasValue)
            {
                Matrix3 transform = positionTransform.Value;
                for (int j = 0; j < loader.VertexList.Count; j++)
                {
                    loader.VertexList[j].Vector = Matrix3.Transform(loader.VertexList[j].Vector, transform);
                }
            }

            // vertex buffer
            var vertexCount = loader.TriangleCount * 3;
            var vertices = new PositionColorTexture[vertexCount];

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

            var buffer = GraphicsBuffer.Create(device, vertices, false);
            return new VertexBufferBinding
            {
                Buffer = buffer,
                Count = vertexCount,
                Declaration = PositionColorTexture.Layout,
                Stride = PositionColorTexture.Layout.Stride,
            };
        }
    }
}
