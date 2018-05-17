using Axiverse.Interface.Graphics.Fonts;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using System.Collections.Generic;
using Device3D = SharpDX.Direct3D11.Device11On12;
using Device3D11 = SharpDX.Direct3D11.Device;
using Device3D12 = SharpDX.Direct3D12.Device;
using DeviceGI = SharpDX.DXGI.Device;
using FactoryDW = SharpDX.DirectWrite.Factory;
using Resource11 = SharpDX.Direct3D11.Resource;
using Resource12 = SharpDX.Direct3D12.Resource;
using ResourceStates = SharpDX.Direct3D12.ResourceStates;

namespace Axiverse.Interface.Graphics
{
    using SharpDX.Direct2D1;

    public class GraphicsDevice2D : GraphicsResource
    {

        private Dictionary<string, FontCollection> m_fontCollections = new Dictionary<string, FontCollection>();
        private Dictionary<Windows.Font, TextFormat> m_fonts = new Dictionary<Windows.Font, TextFormat>();

        public SwapChain SwapChain;

        public Device3D Device3D;
        public Device3D11 Device3D11;
        public SharpDX.Direct3D11.DeviceContext DeviceContext3D;

        public Factory1 Factory;
        public Device Device2D;
        public DeviceContext DeviceContext;

        public FactoryDW FactoryDW;
        public TextFormat TextFormat;

        public Size2F DesktopDpi;
        public FrameResource[] FrameResources;
        //public RenderTarget RenderTarget;

        public ResourceFontLoader FontLoader;
        public FontCollection FontCollection;

        public class FrameResource
        {
            public Resource11 WrappedBackBuffer;
            public Resource12 RenderTarget;
            public Surface Surface;
            public Bitmap1 Bitmap;
        }

        public GraphicsDeviceContext2D canvas;
        public Windows.Canvas Canvas => canvas;
        public SolidColorBrush Brush;

        public RoundedRectangleGeometry RoundedRectangleGeometry;

        private GraphicsDevice2D(GraphicsDevice device) : base(device)
        {

        }

        public void Initialize(SwapChain swapChain)
        {
            // https://msdn.microsoft.com/en-us/library/windows/desktop/mt186590(v=vs.85).aspx
            SwapChain = swapChain;
            Device3D11 = Device3D11.CreateFromDirect3D12(
                Device.NativeDevice,
                DeviceCreationFlags.BgraSupport,// | DeviceCreationFlags.Debug,
                null,
                null,
                swapChain.NativeCommandQueue);

            DeviceContext3D = Device3D11.ImmediateContext;
            Device3D = Device3D11.QueryInterface<Device3D>();

            // create d2d/directwrite
            using (var factory = new Factory(FactoryType.SingleThreaded))
            {
                Factory = factory.QueryInterface<Factory1>();
            }


            RoundedRectangleGeometry = new RoundedRectangleGeometry(
                Factory,
                new RoundedRectangle()
                {
                    RadiusX = 32,
                    RadiusY = 32,
                    Rect = new RectangleF(128, 128, 500 - 128 * 2, 500 - 128 * 2)
                });


            // direct write
            FactoryDW = new FactoryDW(SharpDX.DirectWrite.FactoryType.Shared);
            FontLoader = new ResourceFontLoader(FactoryDW, @"Fonts");
            FontCollection = new FontCollection(FactoryDW, FontLoader, FontLoader.Key);

            TextFormat = new TextFormat(
                FactoryDW,
                "Material-Design-Iconic-Font",
                FontCollection,
                SharpDX.DirectWrite.FontWeight.Normal,
                FontStyle.Normal,
                FontStretch.Normal,
                30)
            {
                TextAlignment = TextAlignment.Leading,
                ParagraphAlignment = ParagraphAlignment.Near,
            };

            DeviceContextOptions deviceOptions = DeviceContextOptions.None;
            using (var deviceGI = Device3D.QueryInterface<DeviceGI>())
            {
                Device2D = new Device(Factory, deviceGI);
                DeviceContext = new DeviceContext(Device2D, deviceOptions);
            }

            DesktopDpi = Factory.DesktopDpi;
            //InitializeFrames(Device.NativeDevice, Renderer.RenderTarget);
        }

        public void InitializeFrames(Device3D12 device3D12, RenderTarget renderTarget)
        {
            canvas = new GraphicsDeviceContext2D(DeviceContext);

            Brush = new SolidColorBrush(DeviceContext, SharpDX.Color.White);

            var properties = new BitmapProperties1(
                new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
                DesktopDpi.Width,
                DesktopDpi.Height,
                BitmapOptions.Target | BitmapOptions.CannotDraw);

            FrameResources = new FrameResource[SwapChain.BufferCount];
            for (int i = 0; i < SwapChain.BufferCount; i++)
            {
                var frameResource = new FrameResource();
                FrameResources[i] = frameResource;
                frameResource.RenderTarget = SwapChain.NativeBackBuffers[i];

                Device3D.CreateWrappedResource(
                    SwapChain.NativeBackBuffers[i],
                    new D3D11ResourceFlags()
                    {
                        BindFlags = (int)BindFlags.RenderTarget
                    },
                    (int)ResourceStates.RenderTarget,
                    (int)ResourceStates.Present,
                    Utilities.GetGuidFromType(typeof(Resource11)),
                    out frameResource.WrappedBackBuffer);

                frameResource.Surface = frameResource.WrappedBackBuffer.QueryInterface<Surface>();
                frameResource.Bitmap = new Bitmap1(DeviceContext, frameResource.Surface, properties);
            }

            // https://msdn.microsoft.com/en-us/library/windows/desktop/dd370966(v=vs.85).aspx#resizing_a_dxgi_surface_render_target
            // https://msdn.microsoft.com/en-us/library/windows/desktop/bb205075(v=vs.85).aspx#Care_and_Feeding_of_the_Swap_Chain
            // https://msdn.microsoft.com/en-us/library/windows/desktop/bb205075(v=vs.85).aspx#Handling_Window_Resizing
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                DisposeFrames();
            }
            base.Dispose(disposing);
        }

        public void DisposeFrames()
        {
            Canvas.Dispose();
            Brush?.Dispose();
            DeviceContext.Target = null;
            DeviceContext3D.OutputMerger.SetRenderTargets((RenderTargetView)null);
            DeviceContext3D.ClearState();

            foreach (var resource in FrameResources)
            {
                resource.Bitmap.Dispose();
                resource.Surface.Dispose();
                resource.WrappedBackBuffer.Dispose();
                resource.RenderTarget.Dispose();
            }

            DeviceContext3D.Flush();

            /*
            DeviceDebug debug = Device3D11.QueryInterface<DeviceDebug>();
            debug.ReportLiveDeviceObjects(ReportingLevel.Detail);
            debug.Dispose();
            */
        }

        public static GraphicsDevice2D Create(GraphicsDevice device, SwapChain swapChain)
        {
            var result = new GraphicsDevice2D(device);
            result.Initialize(swapChain);
            return result;
        }
    }
}
