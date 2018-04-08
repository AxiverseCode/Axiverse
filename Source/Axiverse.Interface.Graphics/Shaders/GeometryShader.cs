using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using Axiverse.Injection;

namespace Axiverse.Interface.Graphics.Shaders
{
    public class GeometryShader : Shader
    {
        struct ConstantData
        {

        }

        public GeometryShader(GraphicsDevice device) : base(device)
        {
            
        }

        public void Initialize()
        {
            // Define the vertex input layout.
            var inputElementDescs = new[]
            {
                new SharpDX.Direct3D12.InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32B32_Float, 0, 0)
            };

            // Shaders
            var testShaderPath = "../../../../Resources/Engine/Test/test.hlsl";
            var pipelineStateDescription = new PipelineStateDescription()
            {
                InputLayout = new SharpDX.Direct3D12.InputLayoutDescription(inputElementDescs),
                RootSignature = RootSignature.Create(Device),
                VertexShader = ShaderBytecode.CompileFromFile(testShaderPath, "VSMain", "vs_5_0"),
                PixelShader = ShaderBytecode.CompileFromFile(testShaderPath, "PSMain", "ps_5_0"),
            };
            var pipelineState = PipelineState.Create(Device, pipelineStateDescription);
        }

        /// <summary>
        /// binds to cbuffer index - for instanced drawing?
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bindings"></param>
        public void Bind(int index, IBindingProvider bindings)
        {
            binder.SetValues(ref data[index], bindings);
            // write to cbuffer
            int offset = 0; // stride * index
            Utilities.Write(IntPtr.Add(IntPtr.Zero, offset), ref data[index]);
        }

        private ConstantData[] data = new ConstantData[10];
        private Binder binder = new Binder(typeof(ConstantData));
    }
}
