using SharpDX;
using SharpDX.Direct3D12;
using System;
using System.Runtime.InteropServices;

namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// Represents a generic GPU bound buffer.
    /// </summary>
    public class GraphicsBuffer : GraphicsResource
    {
        internal Resource NativeResource;

        internal long GpuHandle => NativeResource.GPUVirtualAddress;

        /// <summary>
        /// Gets the size of the buffer in bytes.
        /// </summary>
        public int Size { get; private set; }

        private GraphicsBuffer(GraphicsDevice device) : base(device)
        {

        }

        /// <summary>
        /// Disposes the <see cref="GraphicsBuffer"/>.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                NativeResource.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Writes data into the buffer at a byte offset.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="offsetInBytes"></param>
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

        /// <summary>
        /// Writes data into the buffer at a index offset.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="offsetIndex"></param>
        /// <param name="data"></param>
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

        /// <summary>
        /// Writes an array of data into the buffer at a byte offset.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="offsetInBytes"></param>
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

        /// <summary>
        /// Initializes a graphics buffer with the specified data.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="data"></param>
        /// <param name="dataStatic"></param>
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

        /// <summary>
        /// Creates a <see cref="GraphicsBuffer"/>.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="size"></param>
        /// <param name="data"></param>
        /// <param name="isStatic"></param>
        /// <returns></returns>
        public static GraphicsBuffer Create(GraphicsDevice device, int size, IntPtr data, bool isStatic)
        {
            var result = new GraphicsBuffer(device);
            result.Initialize(size, data, isStatic);
            return result;
        }

        /// <summary>
        /// Creates a <see cref="GraphicsBuffer"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="device"></param>
        /// <param name="data"></param>
        /// <param name="isStatic"></param>
        /// <returns></returns>
        public static GraphicsBuffer Create<T>(GraphicsDevice device, T[] data, bool isStatic)
            where T : struct
        {
            return Create(device, Utilities.SizeOf(data), Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), isStatic);
        }
    }
}
