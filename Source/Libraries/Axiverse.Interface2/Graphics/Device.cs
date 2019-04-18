using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using SharpDX.Windows;

using Device11 = SharpDX.Direct3D11.Device;
using Buffer11 = SharpDX.Direct3D11.Buffer;
using Texture2D11 = SharpDX.Direct3D11.Texture2D;
using SharpDX.Direct3D;

namespace Axiverse.Interface2
{
    public class Device : IDisposable
    {
        public Device11 NativeDevice { get; private set; }
        public DeviceContext NativeDeviceContext { get; private set; }
        public Canvas Canvas { get; private set; }

        private SwapChain swapChain;

        private RenderTargetView backBufferView;
        private DepthStencilView depthStencilView;

        private RasterizerState rasterizerState;
        public BlendState blendState;
        public DepthStencilState depthStencilState;
        private SamplerState samplerState;

        public RenderForm View { get; set; }

        public bool ResizeBuffers { get; set; }

        public Device(RenderForm form, bool debug = false)
        {
            View = form;
            // SwapChain description
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,//buffer count
                ModeDescription =
                    new ModeDescription(View.ClientSize.Width, View.ClientSize.Height,
                        new Rational(60, 1), Format.B8G8R8A8_UNorm),
                IsWindowed = true,
                OutputHandle = View.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            FeatureLevel[] levels = new FeatureLevel[] { FeatureLevel.Level_11_1 };

            //create device and swapchain
            DeviceCreationFlags flag = DeviceCreationFlags.None | DeviceCreationFlags.BgraSupport;
            if (debug)
                flag = DeviceCreationFlags.Debug;

            Device11.CreateWithSwapChain(DriverType.Hardware, flag, levels, desc, out var nativeDevice, out swapChain);
            NativeDevice = nativeDevice;

            //get context to device
            NativeDeviceContext = NativeDevice.ImmediateContext;


            //Ignore all windows events
            var factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(View.Handle, WindowAssociationFlags.IgnoreAll);

            //Setup handler on resize form
            View.UserResized += (sender, args) => ResizeBuffers = true;


            {
                Utilities.Dispose(ref rasterizerState);
                RasterizerStateDescription rasterDescription = RasterizerStateDescription.Default();
                //rasterDescription.CullMode = CullMode.Back;
                rasterizerState = new RasterizerState(NativeDevice, rasterDescription);
                NativeDeviceContext.Rasterizer.State = rasterizerState;
            }

            {

                Utilities.Dispose(ref blendState);
                BlendStateDescription description = BlendStateDescription.Default();
                blendState = new BlendState(NativeDevice, description);
            }

            {

                Utilities.Dispose(ref depthStencilState);
                DepthStencilStateDescription description = DepthStencilStateDescription.Default();
                description.DepthComparison = Comparison.LessEqual;
                description.IsDepthEnabled = true;

                depthStencilState = new DepthStencilState(NativeDevice, description);
            }

            {
                Utilities.Dispose(ref samplerState);
                SamplerStateDescription description = SamplerStateDescription.Default();
                description.Filter = Filter.MinMagMipLinear;
                description.AddressU = TextureAddressMode.Wrap;
                description.AddressV = TextureAddressMode.Wrap;
                samplerState = new SamplerState(NativeDevice, description);
            }

            {
                NativeDeviceContext.Rasterizer.State = rasterizerState;
                NativeDeviceContext.PixelShader.SetSampler(0, samplerState);
                NativeDeviceContext.PixelShader.SetSampler(1, samplerState);
                NativeDeviceContext.PixelShader.SetSampler(2, samplerState);
                NativeDeviceContext.PixelShader.SetSampler(3, samplerState);
                NativeDeviceContext.PixelShader.SetSampler(4, samplerState);
                NativeDeviceContext.PixelShader.SetSampler(5, samplerState);
                NativeDeviceContext.PixelShader.SetSampler(6, samplerState);
                NativeDeviceContext.OutputMerger.SetBlendState(blendState);
                NativeDeviceContext.OutputMerger.SetDepthStencilState(depthStencilState);
            }

            Canvas = new Canvas(this);
            Resize();
        }

        public void SetBlendState(BlendState blendState)
        {
            NativeDeviceContext.OutputMerger.SetBlendState(blendState);
        }

        public void SetDepthStencil(DepthStencilState depthStencilState)
        {
            NativeDeviceContext.OutputMerger.SetDepthStencilState(depthStencilState);
        }

        public void Start()
        {
            if (ResizeBuffers)
            {
                Resize();
            }
        }

        private void Resize()
        {
            // Dispose all previous allocated resources
            Canvas.Release();
            Utilities.Dispose(ref backBufferView);
            Utilities.Dispose(ref depthStencilView);


            if (View.ClientSize.Width == 0 || View.ClientSize.Height == 0)
                return;

            // Resize the backbuffer
            swapChain.ResizeBuffers(1, View.ClientSize.Width, View.ClientSize.Height, Format.B8G8R8A8_UNorm, SwapChainFlags.None);

            // Get the backbuffer from the swapchain
            var backBufferTexture = swapChain.GetBackBuffer<Texture2D11>(0);

            //update font
            Canvas.UpdateResources(backBufferTexture);

            // Backbuffer
            backBufferView = new RenderTargetView(NativeDevice, backBufferTexture);
            backBufferTexture.Dispose();

            // Depth buffer

            var depthStencilTexture = new Texture2D11(NativeDevice, new Texture2DDescription()
            {
                Format = Format.D16_UNorm,
                ArraySize = 1,
                MipLevels = 1,
                Width = View.ClientSize.Width,
                Height = View.ClientSize.Height,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            });


            // Create the depth buffer view
            depthStencilView = new DepthStencilView(NativeDevice, depthStencilTexture);
            depthStencilTexture.Dispose();

            //SetDefaultTargers();

            // Set Default Targets
            NativeDeviceContext.Rasterizer.SetViewport(0, 0, View.ClientSize.Width, View.ClientSize.Height);
            NativeDeviceContext.OutputMerger.SetTargets(depthStencilView, backBufferView);

            // End resize
            //MustResize = false;
        }

        /// <summary>
        /// Clear backbuffer and zbuffer
        /// </summary>
        /// <param name="color">background color</param>
        public void Clear(Color4 color)
        {
            NativeDeviceContext.ClearRenderTargetView(backBufferView, color);
            NativeDeviceContext.ClearDepthStencilView(depthStencilView, DepthStencilClearFlags.Depth, 1.0F, 0);
        }

        /// <summary>
        /// Present scene to screen
        /// </summary>
        public void Present()
        {
            swapChain.Present(0, PresentFlags.None);
        }

        /// <summary>
        /// Create a constant buffer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Buffer11 CreateBuffer<T>() where T : struct
        {
            return new Buffer11(NativeDevice,
                Utilities.SizeOf<T>(),
                ResourceUsage.Default, BindFlags.ConstantBuffer,
                CpuAccessFlags.None, ResourceOptionFlags.None, 0);
        }

        /// <summary>
        /// Update constant buffer
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="buffer">Buffer to update</param>
        /// <param name="data">Data to write inside buffer</param>
        public void UpdateData<T>(Buffer11 buffer, T data) where T : struct
        {
            NativeDeviceContext.UpdateSubresource(ref data, buffer);
        }

        public void Dispose()
        {
            Canvas.Dispose();

            rasterizerState.Dispose();
            blendState.Dispose();
            depthStencilState.Dispose();
            samplerState.Dispose();

            backBufferView.Dispose();
            depthStencilView.Dispose();
            swapChain.Dispose();
            NativeDeviceContext.Dispose();
            NativeDevice.Dispose();
        }
    }
}
