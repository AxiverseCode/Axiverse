using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    public class Buffer : GraphicsResource
    {
        private Resource mUploadHeap;

        private IndexBufferView nativeIndexBufferView;
        private VertexBufferView nativeVertexBufferView;

        public IndexBufferView NativeIndexBufferView => nativeIndexBufferView;
        public VertexBufferView NativeVertexBufferView => nativeVertexBufferView;

        public Buffer(GraphicsDevice device) : base(device)
        {

        }

        private void InitHeaps(GraphicsCommandList list, int size, IntPtr data, bool dataStatic = true)
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

        public void InitAsIndexBuffer(GraphicsCommandList list,int size,IntPtr data,bool dataStatic = true)
        {
            InitHeaps(list, size, data, dataStatic);

            nativeIndexBufferView.BufferLocation = mUploadHeap.GPUVirtualAddress; // check if its static
            nativeIndexBufferView.Format         = SharpDX.DXGI.Format.R32_UInt;
            nativeIndexBufferView.SizeInBytes    = size;
        }

        public void InitAsVertexBuffer(GraphicsCommandList list, int size,int vertexSize, IntPtr data, bool dataStatic = true)
        {
            InitHeaps(list, size, data, dataStatic);

            nativeVertexBufferView.BufferLocation    = mUploadHeap.GPUVirtualAddress; // check if its static
            nativeVertexBufferView.SizeInBytes       = size;
            nativeVertexBufferView.StrideInBytes     = vertexSize;
        }
    }
}
