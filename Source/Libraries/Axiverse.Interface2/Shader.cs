using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public InputLayout Layout { get; set; }

        public Shader(Device device, string path, string vertexEntry, string pixelEntry, InputElement[] elements)
        {
            Device = device;
            var vsBytecode = ShaderBytecode.CompileFromFile(path, vertexEntry, "vs_5_0");
            VertexShader = new VertexShader(device.NativeDevice, vsBytecode);

            var psBytecode = ShaderBytecode.CompileFromFile(path, pixelEntry, "ps_5_0");
            PixelShader = new PixelShader(device.NativeDevice, psBytecode);


            var signature = ShaderSignature.GetInputSignature(vsBytecode);
            Layout = new InputLayout(device.NativeDevice, signature, elements);
        }

        /// <summary>
        /// Applica lo shader
        /// </summary>
        public void Apply()
        {
            Device.NativeDeviceContext.InputAssembler.InputLayout = Layout;
            Device.NativeDeviceContext.VertexShader.Set(VertexShader);
            Device.NativeDeviceContext.PixelShader.Set(PixelShader);
            Device.NativeDeviceContext.GeometryShader.Set(null);
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
            PixelShader?.Dispose();
            Layout?.Dispose();
        }
    }
}
