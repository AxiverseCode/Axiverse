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
        internal Resource NativeResource;

        internal long GpuHandle => NativeResource.GPUVirtualAddress;

        public int Size { get; private set; }

        private GraphicsBuffer(GraphicsDevice device) : base(device)
        {

        }

        public void Write<T>(ref T data, int offsetInBytes = 0)
            where T : struct
        {
            Range range = new Range
            {
                Begin = offsetInBytes,
                End = offsetInBytes + Utilities.SizeOf<T>()
            };

            if (range.End > Size)
            {
                throw new ArgumentException("Data exceeds size");
            }

            IntPtr mapPtr = NativeResource.Map(0, range);
            Marshal.StructureToPtr(data, mapPtr, false);
            NativeResource.Unmap(0, range);
        }

        public void Write<T>(int offsetIndex, ref T data)
            where T : struct
        {
            var size = Utilities.SizeOf<T>();
            Range range = new Range
            {
                Begin = offsetIndex * size,
                End = offsetIndex * size + size
            };

            if (range.End > Size)
            {
                throw new ArgumentException("Data exceeds size");
            }

            IntPtr mapPtr = NativeResource.Map(0, range);
            Marshal.StructureToPtr(data, mapPtr, false);
            NativeResource.Unmap(0, range);
        }

        public void Write<T>(T[] data, int offsetInBytes = 0)
            where T : struct
        {
            Range range = new Range
            {
                Begin = offsetInBytes,
                End = offsetInBytes + Utilities.SizeOf(data)
            };

            if (range.End > Size)
            {
                throw new ArgumentException("Data exceeds size");
            }

            IntPtr mapPtr = NativeResource.Map(0, range);
            IntPtr dataPtr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);
            Utilities.CopyMemory(mapPtr, dataPtr, range.End - range.Begin);
            NativeResource.Unmap(0, range);
        }

        private void Initialize(int size, IntPtr data, bool dataStatic = true)
        {
            Size = size;

            NativeResource = Device.NativeDevice.CreateCommittedResource
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
            IntPtr pData = NativeResource.Map(0, range);
            {
                Utilities.CopyMemory(pData, data, size);
            }
            NativeResource.Unmap(0, null);

            // If it is static, we can upload it to a GPU heap.
            // NOTE: For now we only have dynamic stuff.
            if (dataStatic)
            {

            }
        }

        public static GraphicsBuffer Create(GraphicsDevice device, int size, IntPtr data, bool isStatic)
        {
            var result = new GraphicsBuffer(device);
            result.Initialize(size, data, isStatic);
            return result;
        }

        public static GraphicsBuffer Create<T>(GraphicsDevice device, T[] data, bool isStatic)
            where T: struct
        {
            return Create(device, Utilities.SizeOf(data), Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), isStatic);
        }
    }
}
