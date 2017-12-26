using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D;
using SharpDX.DXGI;

namespace Axiverse.Interface.Graphics
{
    using SharpDX.Direct3D12;

    public class Mesh<T> where T: struct, IVertex
    {
        // bound pass ref

        // geometry
        //      vertex signature
        //      vertex buffer ref
        //      index buffer ref
        //      draw calls ref

        // material (determines pipeline in conjunction to geometry signature)
        //      constant buffers
        //      textures
        //      samplers
        //      shaders

        // pending jobs as dependencies

        public Renderer Renderer;
        public SharpDX.Direct3D12.Resource VertexBufferResource;
        public SharpDX.Direct3D12.Resource IndexBufferResource;
        public VertexBufferView VertexBufferView;
        public IndexBufferView IndexBufferView;
        public InputLayoutDescription InputLayoutDescription;
        public int VertexCount;
        public int IndexCount;
        public int Count;

        public Mesh(Renderer renderer)
        {
            Renderer = renderer;

            T type = new T();
            InputLayoutDescription = type.Description;
        }

        public virtual void Populate(Tuple<uint[], T[]> data)
        {
            Populate(data.Item2, data.Item1);
        }

        public virtual void Populate(T[] vertices, uint[] indices)
        {
            var device = Renderer.Device;

            VertexCount = vertices.Length;

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
                StrideInBytes = Utilities.SizeOf<T>(),
                SizeInBytes = vertexBufferSize,
            };

            if (indices != null)
            {
                IndexCount = indices.Length;

                var indexBufferSize = Utilities.SizeOf(indices);
                // indexBuffer = device.CreateCommittedResource(new HeapProperties(HeapType.Default), HeapFlags.None, ResourceDescription.Buffer(indexBufferSize), ResourceStates.GenericRead);
                IndexBufferResource = device.CreateCommittedResource(new HeapProperties(HeapType.Upload), HeapFlags.None, ResourceDescription.Buffer(indexBufferSize), ResourceStates.GenericRead);

                IntPtr indexData = IndexBufferResource.Map(0);
                Utilities.Write(indexData, indices, 0, indices.Length);
                IndexBufferResource.Unmap(0);

                // index buffer view
                IndexBufferView = new IndexBufferView()
                {
                    BufferLocation = IndexBufferResource.GPUVirtualAddress,
                    Format = Format.R32_UInt,
                    SizeInBytes = indexBufferSize,
                };
            }
        }


        public virtual void Draw(GraphicsCommandList commandList)
        {
            commandList.PrimitiveTopology = PrimitiveTopology.TriangleList;
            commandList.SetVertexBuffer(0, VertexBufferView);

            if (IndexBufferResource != null)
            {
                commandList.SetIndexBuffer(IndexBufferView);
                commandList.DrawIndexedInstanced(IndexCount, 1, 0, 0, 0);
            }
            else
            {
                commandList.DrawInstanced(VertexCount, 1, 0, 0);
            }
        }

        public virtual void Dispose()
        {
            VertexBufferResource?.Dispose();
            IndexBufferResource?.Dispose();
        }
    }
}
