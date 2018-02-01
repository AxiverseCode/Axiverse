using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D12;

namespace Axiverse.Interface.Engine.Rendering
{
    class Buffer
    {
        private Resource mUploadHeap;
        private Resource mGPUHeap;
        public GpuDescriptorHandle View;

        void InitAsIndexBuffer(ref Device device)
        {
            mUploadHeap = device.CreateCommittedResource
            (
                    new HeapProperties(HeapType.Upload),
                    HeapFlags.None,
                    ResourceDescription.Buffer(100),
                    ResourceStates.CopySource
            );
        }
           
        void InitAsVertexBuffer()
        {

        }
    }
}
