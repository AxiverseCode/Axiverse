using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    public class Buffer
    {
        private Resource mUploadHeap;

        private IndexBufferView mIndexBufferView;
        private VertexBufferView mVertexBufferView;

        private void InitHeaps(Device device, GraphicsCommandList list, int size, IntPtr data, bool dataStatic = true)
        {
            mUploadHeap = device.CreateCommittedResource
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

        public void InitAsIndexBuffer(Device device,GraphicsCommandList list,int size,IntPtr data,bool dataStatic = true)
        {
            InitHeaps(device, list, size, data, dataStatic);

            mIndexBufferView.BufferLocation = mUploadHeap.GPUVirtualAddress; // check if its static
            mIndexBufferView.Format         = SharpDX.DXGI.Format.R32_UInt;
            mIndexBufferView.SizeInBytes    = size;
        }

        public void InitAsVertexBuffer(Device device, GraphicsCommandList list, int size,int vertexSize, IntPtr data, bool dataStatic = true)
        {
            InitHeaps(device, list, size, data, dataStatic);

            mVertexBufferView.BufferLocation    = mUploadHeap.GPUVirtualAddress; // check if its static
            mVertexBufferView.SizeInBytes       = size;
            mVertexBufferView.StrideInBytes     = vertexSize;
        }

        public IndexBufferView AsIndexBuffer()
        {
            return mIndexBufferView;
        }

        public VertexBufferView AsVertexBuffer()
        {
            return mVertexBufferView;
        }
    }
}
