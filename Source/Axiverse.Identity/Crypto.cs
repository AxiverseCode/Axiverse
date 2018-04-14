using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Axiverse.Identity
{
    public class Crypto
    {
        private RandomNumberGenerator random = RandomNumberGenerator.Create();

        private byte[] GenerateSalt()
        {
            var result = new byte[32];
            random.GetBytes(result);
            return result;
            
        }

        public static byte[] GetBytes<T>(T value)
        {
            int size = Marshal.SizeOf(value);
            byte[] bytes = new byte[size];

            GCHandle handle = default(GCHandle);
            try
            {
                handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                Marshal.StructureToPtr<T>(value, handle.AddrOfPinnedObject(), false);
            }
            finally
            {
                if (handle.IsAllocated)
                {
                    handle.Free();
                }
            }
            return bytes;
        }

        public static T FromBytes<T>(byte[] bytes)
        {
            T result = default(T);
            GCHandle handle = default(GCHandle);
            try
            {
                handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                result = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            }
            finally
            {
                if (handle.IsAllocated)
                {
                    if (handle.IsAllocated)
                    {
                        handle.Free();
                    }
                }
            }
            return result;
        }

        public static byte[] Hash(byte[] buffer)
        {
            HashAlgorithm algorithm = SHA256.Create();
            byte[] hash = algorithm.ComputeHash(buffer);
            return hash;
        }
    }
}
