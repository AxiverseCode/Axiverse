using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Injection;
using SharpDX;

namespace Axiverse.Interface.Graphics.Shaders
{
    public class ShaderBinding
    {
        List<ResourceBinding> Resources = new List<ResourceBinding>();
        List<ObjectResourceBinding> Objects = new List<ObjectResourceBinding>();

        public void Apply(CommandList commandList, ResourceGroup resources, IBindingProvider bindings)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                var allocation = resources.Allocations[i];
                Objects[i].Apply(bindings, allocation.Data + allocation.Offset);
                //commandList.NativeCommandList.SetGraphicsRootDescriptorTable(Objects[i].ParameterIndex, allocation.Buffer.mUploadHeap.GPUVirtualAddress);
            }

            foreach (var resource in Resources)
            {
                // get descriptor from bindings
                //commandList.NativeCommandList.SetGraphicsRootDescriptorTable(resource.ParameterIndex, 0);
            }
        }
    }

    public class ResourceGroup
    {
        // one cbuffer allocation for each objectresource binding
        public List<GraphicsBufferAllocation> Allocations = new List<GraphicsBufferAllocation>();
    }








    public class ResourceBinding
    {
        // RootParameterIndex
        public int ParameterIndex;

        // Binding Key
        public Key Key;
    }

    public class ObjectResourceBinding
    {
        // RootParameterIndex
        public int ParameterIndex;

        // Binding Key
        public Key Key;

        /// <summary>
        /// Gets a list of the individual field bindings
        /// </summary>
        List<FieldBinding> Bindings = new List<FieldBinding>();

        /// <summary>
        /// Gets the size of the bound object.
        /// </summary>
        public int Size;

        public void Create(Type type)
        {
            Size = Marshal.SizeOf(type);

            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var key = Key.From(field.FieldType, field.Name);
                var offset = Marshal.OffsetOf(type, field.Name);
                var binding = new FieldBinding(key, (int)offset);

                Bindings.Add(binding);
            }
        }

        public void Apply(IBindingProvider bindings, IntPtr data)
        {
            foreach (var binding in Bindings)
            {
                binding.Apply(bindings, data);
            }
        }
    }

    public class FieldBinding
    {
        public Key Key;
        public int Offset;

        public FieldBinding(Key key, int offset)
        {
            Contract.Requires(key.Type.IsValueType, "Data object bindings can only use value types.");
            Key = key;
            Offset = offset;
        }

        public void Apply(IBindingProvider bindings, IntPtr data)
        {
            if (bindings.TryGetValue(Key, out var structure))
            {
                // No deletion of old data because we only allow value types.
                Marshal.StructureToPtr(structure, data + Offset, false);
            }
        }
    }
}
