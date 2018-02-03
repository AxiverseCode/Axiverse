using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D12;

namespace Axiverse.Interface.Engine.Rendering
{
    class Buffer
    {
        private Resource mUploadHeap;
        private Resource mGPUHeap;

        private IndexBufferView mIndexBufferView;
        public VertexBufferView mVertexBufferView;

        void InitAsIndexBuffer(ref Device device,IntPtr data,int dataSize)
        {
            // Upload Heap, CPU can write to it. 
            mUploadHeap = device.CreateCommittedResource
            (
                new HeapProperties(HeapType.Upload),
                HeapFlags.None,
                ResourceDescription.Buffer(dataSize),
                ResourceStates.GenericRead
            );

            // Copy data
            Range mapRange = new Range
            {
                Begin = 0,
                End = 0
            };
            IntPtr pData = mUploadHeap.Map(0,mapRange);
            Utilities.CopyMemory(pData, data, dataSize);
            mUploadHeap.Unmap(0,null);

            // Create a view
            mIndexBufferView = new IndexBufferView
            {
                Format = SharpDX.DXGI.Format.Unknown,
                SizeInBytes = dataSize,
                BufferLocation = mUploadHeap.GPUVirtualAddress
            };
        }
           
        void InitAsVertexBuffer()
        {

        }

        IndexBufferView AsIndexBuffer()
        {
            return mIndexBufferView;
        }

        VertexBufferView AsVertexBuffer()
        {
            return mVertexBufferView;
        }
    }
}
