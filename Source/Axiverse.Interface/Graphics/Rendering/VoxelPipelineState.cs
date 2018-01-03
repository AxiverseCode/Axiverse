using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Axiverse.Resources;
using SharpDX;
using SharpDX.DXGI;

namespace Axiverse.Interface.Graphics
{
    using SharpDX.Direct3D12;

    public class VoxelPipelineState
    {
        private const int MaxObjects = 80;

        [StructLayout(LayoutKind.Explicit, Pack = 16, Size = 256 * (MaxObjects / 4))]
        struct PerDraw
        {
            [FieldOffset(0), MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxObjects)]
            public Matrix[] WorldViewProjection;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 16, Size = 256)]
        struct PerObject
        {
            public Matrix WorldViewProjection;
        }

        public Resource ConstantBuffer;
        public DescriptorHeap ConstantBufferViewShaderResourceViewDescriptorHeap;
        public int ConstantBufferViewShaderResourceViewDescriptorSize;
        public DescriptorHeap SamplerViewDescriptorHeap;
        public IntPtr ConstantBufferData;

        public Renderer Renderer { get; private set; }
        public Device Device { get; private set; }

        public PipelineState PipelineState;
        public RootSignature RootSignature;

        public VoxelPipelineState(Renderer renderer)
        {
            Renderer = renderer;
            Device = renderer.Device;
        }

        public virtual void Load(string path, InputLayoutDescription inputLayoutDescription)
        {
            // root description
            var rootParameters = new RootParameter[]
            {
                new RootParameter(ShaderVisibility.Vertex,
                    new DescriptorRange()
                    {
                        RangeType = DescriptorRangeType.ConstantBufferView,
                        BaseShaderRegister = 0,
                        OffsetInDescriptorsFromTableStart = int.MinValue,
                        DescriptorCount = 1,
                    }),
                new RootParameter(ShaderVisibility.Pixel,
                    new DescriptorRange()
                    {
                        RangeType = DescriptorRangeType.ShaderResourceView,
                        DescriptorCount = 1,
                        OffsetInDescriptorsFromTableStart = int.MinValue,
                        BaseShaderRegister = 0
                    }),
                 new RootParameter(ShaderVisibility.Pixel,
                    new DescriptorRange()
                    {
                        RangeType = DescriptorRangeType.Sampler,
                        BaseShaderRegister = 0,
                        DescriptorCount = 1
                    }),
            };

            var rootSignatureDescription = new RootSignatureDescription(RootSignatureFlags.AllowInputAssemblerInputLayout, rootParameters);
            RootSignature = Renderer.Device.CreateRootSignature(rootSignatureDescription.Serialize());

            // pixel and vertex shaders
            var shaderFlags = SharpDX.D3DCompiler.ShaderFlags.Debug;
            byte[] shaderBuffer;

            using (var fileStream = Store.Default.Open(path, FileMode.Open))
            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                var shaderString = Encoding.UTF8.GetString(memoryStream.GetBuffer());
                var preamble = Encoding.UTF8.GetPreamble();
                var buffer = memoryStream.GetBuffer();
                shaderBuffer = memoryStream.ToArray();
            }
            var vertexBytecode = SharpDX.D3DCompiler.ShaderBytecode.Compile(shaderBuffer, "VSMain", "vs_5_0", shaderFlags);
            var pixelBytecode = SharpDX.D3DCompiler.ShaderBytecode.Compile(shaderBuffer, "PSMain", "ps_5_0", shaderFlags);

            var vertexShader = new ShaderBytecode(vertexBytecode);
            var pixelShader = new ShaderBytecode(pixelBytecode);

            var rasterizerState = RasterizerStateDescription.Default();
            //rasterizerState.CullMode = CullMode.None;
            //rasterizerState.FillMode = FillMode.Wireframe;

            // pipeline state
            var pipelineStateDescription = new GraphicsPipelineStateDescription()
            {
                InputLayout = inputLayoutDescription,
                RootSignature = RootSignature,
                VertexShader = vertexShader,
                PixelShader = pixelShader,
                RasterizerState = rasterizerState,
                BlendState = BlendStateDescription.Default(),
                DepthStencilFormat = Format.D32_Float,
                DepthStencilState = DepthStencilStateDescription.Default(),
                SampleMask = int.MaxValue,
                PrimitiveTopologyType = PrimitiveTopologyType.Triangle,
                RenderTargetCount = 1,
                Flags = PipelineStateFlags.None,
                SampleDescription = new SampleDescription(1, 0),
                StreamOutput = new StreamOutputDescription()
            };
            pipelineStateDescription.RenderTargetFormats[0] = Format.B8G8R8A8_UNorm;
            //pipelineStateDescription.DepthStencilState.DepthWriteMask = DepthWriteMask.Zero;
            PipelineState = Renderer.Device.CreateGraphicsPipelineState(pipelineStateDescription);









            // constant buffer
            var constantBufferViewShaderResourceViewHeapDescription = new DescriptorHeapDescription
            {
                // first index is used for texture
                DescriptorCount = 2,
                Type = DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView,
                Flags = DescriptorHeapFlags.ShaderVisible,
            };
            ConstantBufferViewShaderResourceViewDescriptorHeap = Device.CreateDescriptorHeap(constantBufferViewShaderResourceViewHeapDescription);
            ConstantBufferViewShaderResourceViewDescriptorSize = Device.GetDescriptorHandleIncrementSize(DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);

            ConstantBuffer = Device.CreateCommittedResource(
                new HeapProperties(HeapType.Upload),
                HeapFlags.None,
                ResourceDescription.Buffer(Utilities.SizeOf<PerDraw>()),
                ResourceStates.GenericRead);
            
            var constantBufferViewDescription = new ConstantBufferViewDescription()
            {
                BufferLocation = ConstantBuffer.GPUVirtualAddress + Utilities.SizeOf<PerDraw>() * 0,
                SizeInBytes = Utilities.SizeOf<PerDraw>(),
            };

            Device.CreateConstantBufferView(
                constantBufferViewDescription,
                ConstantBufferViewShaderResourceViewDescriptorHeap.CPUDescriptorHandleForHeapStart + ConstantBufferViewShaderResourceViewDescriptorSize);
            ConstantBufferData = ConstantBuffer.Map(0);


            // sampler buffer view heap
            SamplerViewDescriptorHeap = Device.CreateDescriptorHeap(new DescriptorHeapDescription()
            {
                DescriptorCount = 1,
                Type = DescriptorHeapType.Sampler,
                Flags = DescriptorHeapFlags.ShaderVisible
            });

            // bind sampler
            Device.CreateSampler(new SamplerStateDescription()
            {
                Filter = Filter.ComparisonAnisotropic,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                MinimumLod = float.MinValue,
                MaximumLod = float.MaxValue,
                MipLodBias = 0,
                MaximumAnisotropy = 16,
                ComparisonFunction = Comparison.Never
            }, SamplerViewDescriptorHeap.CPUDescriptorHandleForHeapStart);
        }

        public void SetTransform(Matrix matrix, int index = 0)
        {
            PerDraw perDraw = new PerDraw
            {
                WorldViewProjection = new Matrix[80]
            };
            perDraw.WorldViewProjection[index] = matrix;

            PerObject perObject = new PerObject
            {
                WorldViewProjection = matrix
            };

            Utilities.Write(ConstantBufferData, ref perObject);
        }

        public void SetTexture(Texture texture)
        {
            Device.CreateShaderResourceView(
                texture.Resource,
                texture.ShaderResourceViewDescription,
                ConstantBufferViewShaderResourceViewDescriptorHeap.CPUDescriptorHandleForHeapStart);
        }

        public void Apply(GraphicsCommandList commandList)
        {
            commandList.SetGraphicsRootSignature(RootSignature);

            commandList.SetDescriptorHeaps(2, new[] { ConstantBufferViewShaderResourceViewDescriptorHeap, SamplerViewDescriptorHeap });
            commandList.SetGraphicsRootDescriptorTable(0, ConstantBufferViewShaderResourceViewDescriptorHeap.GPUDescriptorHandleForHeapStart + ConstantBufferViewShaderResourceViewDescriptorSize); // matrix
            commandList.SetGraphicsRootDescriptorTable(1, ConstantBufferViewShaderResourceViewDescriptorHeap.GPUDescriptorHandleForHeapStart); // texture
            commandList.SetGraphicsRootDescriptorTable(2, SamplerViewDescriptorHeap.GPUDescriptorHandleForHeapStart);
        }

        public virtual void Dispose()
        {
            PipelineState.Dispose();
            RootSignature.Dispose();
        }
    }
}
