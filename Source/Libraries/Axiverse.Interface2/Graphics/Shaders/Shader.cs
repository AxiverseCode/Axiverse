using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Models;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace Axiverse.Interface2
{
    public class Shader : IDisposable
    {
        public Device Device { get; set; }

        public VertexShader VertexShader { get; set; }
        public PixelShader PixelShader { get; set; }
        public GeometryShader GeometryShader { get; set; }


        public ShaderSignature Signature { get; set; }
        public Dictionary<InputElement[], InputLayout> Layouts { get; } = new Dictionary<InputElement[], InputLayout>();

        public InputLayout Layout { get; set; }

        public Shader(Device device, string path, string vertexEntry, string pixelEntry, InputElement[] elements, string geometryEntry = null)
        {
            Device = device;
            var vsBytecode = ShaderBytecode.CompileFromFile(path, vertexEntry, "vs_5_0");
            VertexShader = new VertexShader(device.NativeDevice, vsBytecode);

            if (geometryEntry != null)
            {
                var gsBytecode = ShaderBytecode.CompileFromFile(path, geometryEntry, "gs_5_0");
                GeometryShader = new GeometryShader(device.NativeDevice, gsBytecode);
            }

            var psBytecode = ShaderBytecode.CompileFromFile(path, pixelEntry, "ps_5_0");
            PixelShader = new PixelShader(device.NativeDevice, psBytecode);

            Signature = ShaderSignature.GetInputSignature(vsBytecode);
            Layout = new InputLayout(device.NativeDevice, Signature, elements);
        }

        public Shader(Device device,
            string path,
            string vertexEntry = null,
            string geometryEntry = null,
            string pixelEntry = null)
        {
            Device = device;

            var vsBytecode = ShaderBytecode.CompileFromFile(path, vertexEntry, "vs_5_0");
            VertexShader = new VertexShader(device.NativeDevice, vsBytecode);

            if (geometryEntry != null)
            {
                var gsBytecode = ShaderBytecode.CompileFromFile(path, geometryEntry, "gs_5_0");
                GeometryShader = new GeometryShader(device.NativeDevice, gsBytecode);
            }

            var psBytecode = ShaderBytecode.CompileFromFile(path, pixelEntry, "ps_5_0");
            PixelShader = new PixelShader(device.NativeDevice, psBytecode);

            Signature = ShaderSignature.GetInputSignature(vsBytecode);
        }

        public void Apply(InputElement[] elements)
        {
            if (!Layouts.TryGetValue(elements, out var layout))
            {
                layout = new InputLayout(Device.NativeDevice, Signature, elements);
                Layouts.Add(elements, layout);
            }

            Device.NativeDeviceContext.InputAssembler.InputLayout = layout;
            Device.NativeDeviceContext.VertexShader.Set(VertexShader);
            Device.NativeDeviceContext.PixelShader.Set(PixelShader);
            Device.NativeDeviceContext.GeometryShader.Set(GeometryShader);
            Device.NativeDeviceContext.DomainShader.Set(null);
            Device.NativeDeviceContext.HullShader.Set(null);
        }

        public void Prepare(Model model, Model.Segment segment)
        {

        }

        /// <summary>
        /// Applica lo shader
        /// </summary>
        public void Apply()
        {
            Device.NativeDeviceContext.InputAssembler.InputLayout = Layout;
            Device.NativeDeviceContext.VertexShader.Set(VertexShader);
            Device.NativeDeviceContext.PixelShader.Set(PixelShader);
            Device.NativeDeviceContext.GeometryShader.Set(GeometryShader);
            Device.NativeDeviceContext.DomainShader.Set(null);
            Device.NativeDeviceContext.HullShader.Set(null);
        }

        /// <summary>
        /// Remove Shader from
        /// </summary>
        public void Clear()
        {
            Device.NativeDeviceContext.VertexShader.Set(null);
            Device.NativeDeviceContext.PixelShader.Set(null);
            Device.NativeDeviceContext.GeometryShader.Set(null);
            Device.NativeDeviceContext.DomainShader.Set(null);
            Device.NativeDeviceContext.HullShader.Set(null);
        }

        public void Dispose()
        {
            VertexShader?.Dispose();
            GeometryShader?.Dispose();
            PixelShader?.Dispose();
            Layout?.Dispose();
        }
    }
}
