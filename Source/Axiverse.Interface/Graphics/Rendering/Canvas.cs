using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

using Device3D = SharpDX.Direct3D11.Device11On12;
using Device3D11 = SharpDX.Direct3D11.Device;
using Device3D12 = SharpDX.Direct3D12.Device;
using DeviceGI = SharpDX.DXGI.Device;
using FactoryDW = SharpDX.DirectWrite.Factory;
using ResourceStates = SharpDX.Direct3D12.ResourceStates;
using Resource11 = SharpDX.Direct3D11.Resource;
using Resource12 = SharpDX.Direct3D12.Resource;

using Axiverse.Interface.Graphics.Fonts;

namespace Axiverse.Interface.Graphics
{
    using Axiverse.Mathematics;
    using SharpDX.Direct2D1;

    // TODO: Merge code into WindowsPipeline
    public class Canvas
    {
        public Renderer Renderer;

        private Dictionary<string, FontCollection> m_fontCollections = new Dictionary<string, FontCollection>();
        private Dictionary<Windows.Font, TextFormat> m_fonts = new Dictionary<Windows.Font, TextFormat>();

        public Device3D Device3D;
        public Device3D11 Device3D11;
        public SharpDX.Direct3D11.DeviceContext DeviceContext3D;

        public Factory1 Factory;
        public Device Device;
        public DeviceContext DeviceContext;

        public FactoryDW FactoryDW;
        public TextFormat TextFormat;

        public Size2F DesktopDpi;
        public FrameResource[] FrameResources;
        public RenderTarget RenderTarget;

        public ResourceFontLoader FontLoader;
        public FontCollection FontCollection;

        public class FrameResource
        {
            public Resource11 WrappedBackBuffer;
            public Resource12 RenderTarget;
            public Surface Surface;
            public Bitmap1 Bitmap;
        }

        public Compositor Compositor;
        public SolidColorBrush Brush;

        public RoundedRectangleGeometry RoundedRectangleGeometry;

        public void Initialize(Renderer renderer)
        {
            // https://msdn.microsoft.com/en-us/library/windows/desktop/mt186590(v=vs.85).aspx
            Renderer = renderer;
            RenderTarget = Renderer.RenderTarget;
            Device3D11 = Device3D11.CreateFromDirect3D12(
                Renderer.Device,
                DeviceCreationFlags.BgraSupport,// | DeviceCreationFlags.Debug,
                null,
                null,
                Renderer.CommandQueue);

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
                FontWeight.Normal,
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
                Device = new Device(Factory, deviceGI);
                DeviceContext = new DeviceContext(Device, deviceOptions);
            }

            DesktopDpi = Factory.DesktopDpi;
            InitializeFrames(Renderer.Device, Renderer.RenderTarget);
        }

        public void InitializeFrames(Device3D12 device3D12, RenderTarget renderTarget)
        {
            Compositor = new Compositor(DeviceContext);

            Brush = new SolidColorBrush(DeviceContext, Color.White);

            var properties = new BitmapProperties1(
                new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
                DesktopDpi.Width,
                DesktopDpi.Height,
                BitmapOptions.Target | BitmapOptions.CannotDraw);

            FrameResources = new FrameResource[RenderTarget.FrameCount];
            for (int i = 0; i < RenderTarget.FrameCount; i++)
            {
                var frameResource = new FrameResource();
                FrameResources[i] = frameResource;
                frameResource.RenderTarget = renderTarget.RenderTargets[i];

                Device3D.CreateWrappedResource(
                    renderTarget.RenderTargets[i],
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

        public void DisposeFrames()
        {

            Compositor.Dispose();
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

        Windows.Window ui;
        Windows.Control control;
        public Canvas(Windows.Window userInterface)
        {
            
            ui = userInterface;

            Windows.Control.DefaultFont = new Windows.Font("Open Sans", 16, Windows.FontWeight.Normal);

            control = new Windows.Dialog();
            control.Bounds = new Rectangle(10, 10, 400, 400);
            control.BackgroundColor = new Windows.Color(.2f);
            ui.Children.Add(control);

            control.Children.Add(new Windows.Control()
            {
                Bounds = new Rectangle(10, 50, 50, 50),
                BackgroundColor = Windows.Colors.Yellow,
            });

            {
                var z = new Windows.Button()
                {
                    Location = new Vector2(100, 50),
                };

                control.Children.Add(z);
            }
        }

        public void Draw()
        {

            FrameResource frameResource = FrameResources[RenderTarget.FrameIndex];

            Device3D.AcquireWrappedResources(new[] { frameResource.WrappedBackBuffer }, 1);
            var rectangle = new RectangleF(0, 0, 200, 200);

            DeviceContext.Target = frameResource.Bitmap;

            DeviceContext.BeginDraw();

            DeviceContext.Transform = Matrix3x2.Identity;
            ui.Draw(this);

            //var b = new RectangleF(50, 50, 100, 100);
            //DeviceContext.PushAxisAlignedClip(b, AntialiasMode.PerPrimitive);

            // https://github.com/Microsoft/DirectX-Graphics-Samples/issues/212
            //DeviceContext.FillRectangle(b, Brush);
            //DeviceContext.Clear(new Color4(1, 1, 1, 1));
            //DeviceContext.PopAxisAlignedClip();
            //DeviceContext.Clear(new Color4(1, 1, 1, 1));

            //DeviceContext.Flush();
            DeviceContext.EndDraw();





            Device3D.ReleaseWrappedResources(new[] { frameResource.WrappedBackBuffer }, 1);
            DeviceContext3D.Flush();



            /*
            PathGeometry geometry = new PathGeometry(Factory);
            GeometrySink sink = geometry.Open();

            sink.Close();
            */

        }

        public void DrawText(string text, RawRectangleF bounds, RawColor4 color)
        {
            Brush.Color = color;
            DeviceContext.DrawText("\uf202" + text, TextFormat, bounds, Brush);
        }

        public void Dispose()
        {
            foreach (var resource in FrameResources)
            {
                resource.Bitmap.Dispose();
                resource.Surface.Dispose();
                resource.WrappedBackBuffer.Dispose();
            }

            Brush.Dispose();
            RoundedRectangleGeometry.Dispose();

            FontCollection?.Dispose();
            FontLoader?.Dispose();

            TextFormat?.Dispose();
            FactoryDW?.Dispose();

            DeviceContext.Dispose();
            Device.Dispose();
            Factory.Dispose();

            Device3D.Dispose();
            DeviceContext3D.Dispose();
            Device3D11.Dispose();


        }
    }
}
