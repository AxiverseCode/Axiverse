using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.DXGI;

namespace Axiverse.Interface.Graphics
{
    using SharpDX.Direct3D12;

    public class Geometry<T> where T : struct
    {
        public Resource VertexBufferResource;
        public Resource IndexBufferResource;
        public VertexBufferView VertexBufferView;
        public IndexBufferView IndexBufferView;
        public InputLayoutDescription InputLayoutDescription;
        public DrawSet[] DrawSet;
        public IndexedDrawSet[] IndexedDrawSet;

        public Geometry(T[] vertices)
        {
            Device device = null;
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

            // index buffer
            var indices = new uint[] {
                0, 1, 2, 0, 2, 3,		// front
			    4, 5, 6, 4, 6, 7,		// back
			    8, 9, 10, 8, 10, 11,	// top
			    12, 13, 14, 12, 14, 15,	// bottom
			    16, 17, 18, 16, 18, 19,	// right
			    20, 21, 22, 20, 22, 23  // left
            };
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

    public struct DrawSet
    {

    }

    public struct IndexedDrawSet
    {

    }
}
