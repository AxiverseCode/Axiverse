using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D12;
using SharpDX.Mathematics.Interop;

namespace Axiverse.Interface.Graphics
{
    public class GraphicsBuffer : GraphicsResource
    {
        private Resource mUploadHeap;

        private IndexBufferView nativeIndexBufferView;
        private VertexBufferView nativeVertexBufferView;

        public IndexBufferView NativeIndexBufferView => nativeIndexBufferView;
        public VertexBufferView NativeVertexBufferView => nativeVertexBufferView;

        private GraphicsBuffer(GraphicsDevice device) : base(device)
        {

        }

        private void InitializeHeaps(int size, IntPtr data, bool dataStatic = true)
        {
            mUploadHeap = Device.NativeDevice.CreateCommittedResource
            (
                new HeapProperties(HeapType.Upload),
                HeapFlags.None,
                ResourceDescription.Buffer(size),
                ResourceStates.GenericRead
            );

            // Here we map the upload heap. Note the ranges, the first (0,0) indicates that we wont read 
            // any memory and the second (null) that we wrote the entire resource
            Range range = new Range();
            range.Begin = 0;
            range.End = 0;
            IntPtr pData = mUploadHeap.Map(0, range);
            {
                Utilities.CopyMemory(pData, data, size);
            }
            mUploadHeap.Unmap(0, null);

            // If it is static, we can upload it to a GPU heap.
            // NOTE: For now we only have dynamic stuff.
            if (dataStatic)
            {

            }
        }

        private void InitializeAsIndexBuffer(int size, IntPtr data, bool dataStatic = true)
        {
            InitializeHeaps(size, data, dataStatic);

            nativeIndexBufferView.BufferLocation = mUploadHeap.GPUVirtualAddress; // check if its static
            nativeIndexBufferView.Format = SharpDX.DXGI.Format.R32_UInt;
            nativeIndexBufferView.SizeInBytes = size;
        }

        private void InitializeAsVertexBuffer(int size, int vertexSize, IntPtr data, bool dataStatic = true)
        {
            InitializeHeaps(size, data, dataStatic);

            nativeVertexBufferView.BufferLocation = mUploadHeap.GPUVirtualAddress; // check if its static
            nativeVertexBufferView.SizeInBytes = size;
            nativeVertexBufferView.StrideInBytes = vertexSize;
        }

        public static GraphicsBuffer CreateIndexBuffer(GraphicsDevice device, int size, IntPtr data, bool dataStatic = true)
        {
            var buffer = new GraphicsBuffer(device);
            buffer.InitializeAsIndexBuffer(size, data, dataStatic);
            return buffer;
        }

        public static GraphicsBuffer CreateIndexBuffer(GraphicsDevice device, int[] data, bool dataStatic = true)
        {
            var buffer = new GraphicsBuffer(device);
            buffer.InitializeAsIndexBuffer(
                Utilities.SizeOf(data),
                Marshal.UnsafeAddrOfPinnedArrayElement(data, 0),
                dataStatic);
            return buffer;
        }

        public static GraphicsBuffer CreateVertexBuffer(GraphicsDevice device, int size, int vertexSize, IntPtr data, bool dataStatic = true)
        {
            var buffer = new GraphicsBuffer(device);
            buffer.InitializeAsVertexBuffer(size, vertexSize, data, dataStatic);
            return buffer;
        }

        public static GraphicsBuffer CreateVertexBuffer<T>(GraphicsDevice device, T[] data, int stride = 1, bool isStatic = true)
            where T : struct
        {
            var buffer = new GraphicsBuffer(device);
            buffer.InitializeAsVertexBuffer(
                Utilities.SizeOf(data),
                Utilities.SizeOf<T>() * stride,
                Marshal.UnsafeAddrOfPinnedArrayElement(data, 0),
                isStatic);
            return buffer;
        }
    }
}
