using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Texture2D11 = SharpDX.Direct3D11.Texture2D;
using Device2D = SharpDX.Direct2D1.Device;
using Factory2D = SharpDX.Direct2D1.Factory1;
using DeviceGI = SharpDX.DXGI.Device;

namespace Axiverse.Interface2
{
    public class Canvas : IDisposable
    {
        public Device Device { get; }
        public Device2D NativeDevice { get; set; }
        public Factory2D NativeFactory { get; set; }
        public DeviceContext NativeDeviceContext { get; set; }

        public Bitmap1 Target { get; set; }
        public TextFormat TextFormat { get; set; }
        public SolidColorBrush Brush { get; set; }

        private string fontName = "Calibri";
        private int fontSize = 14;
        private Color fontColor = Color.White;

        /// <summary>
        /// Create a batch manager for drawing text and sprite
        /// </summary>
        /// <param name="device">Device pointer</param>
        internal Canvas(Device device)
        {
            Device = device;

            NativeFactory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded, DebugLevel.Information);

            using (var dxgiDevice = device.NativeDevice.QueryInterface<DeviceGI>())
                NativeDevice = new Device2D(NativeFactory, dxgiDevice);

            NativeDeviceContext = new DeviceContext(NativeDevice, DeviceContextOptions.None);

        }


        /// <summary>
        /// Begin a 2D drawing session
        /// </summary>
        public void Begin()
        {
            NativeDeviceContext.BeginDraw();
        }

        /// <summary>
        /// End drawing session
        /// </summary>
        public void End()
        {
            NativeDeviceContext.EndDraw();
        }

        /// <summary>
        /// Update all resources
        /// </summary>
        /// <param name="backBuffer">BackBuffer</param>
        internal void UpdateResources(Texture2D11 backBuffer)
        {
            using (var surface2D = backBuffer.QueryInterface<Surface>())
            {

                var dpi = NativeFactory.DesktopDpi;
                Target = new Bitmap1(NativeDeviceContext, surface2D,
                    new BitmapProperties1(new PixelFormat(
                        Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied), dpi.Height, dpi.Width, BitmapOptions.CannotDraw | BitmapOptions.Target));
                NativeDeviceContext.Target = Target;
            }

            InitializeFont();
        }

        private void InitializeFont()
        {
            var factoryWrite = new SharpDX.DirectWrite.Factory();
            TextFormat = new TextFormat(factoryWrite, fontName, fontSize) {
                TextAlignment = TextAlignment.Leading,
                ParagraphAlignment = ParagraphAlignment.Near
            };
            Brush = new SolidColorBrush(NativeDeviceContext, fontColor);
            factoryWrite.Dispose();
        }

        /// <summary>
        /// Draw text
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="x">Left position</param>
        /// <param name="y">Top position</param>
        /// <param name="width">Max width</param>
        /// <param name="height">Max heigh</param>
        public void DrawString(string text, int x, int y, int width = 800, int height = 600)
        {
            NativeDeviceContext.DrawText(text, TextFormat, new RawRectangleF(x, y, width, height), Brush);
        }

        public void DrawImage(Image2D image, Vector2 location)
        {
            var previous = NativeDeviceContext.Transform;
            NativeDeviceContext.Transform = Matrix3x2.Multiply(NativeDeviceContext.Transform, Matrix3x2.Translation(new SharpDX.Vector2(location.X, location.Y)));
            NativeDeviceContext.DrawBitmap(image.nativeBitmap, 1.0f, BitmapInterpolationMode.Linear);
            NativeDeviceContext.Transform = previous;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            NativeDeviceContext.Target = null;
            TextFormat?.Dispose();
            TextFormat = null;
            Brush?.Dispose();
            Brush = null;
            Target?.Dispose();
            Target = null;
        }
    }
}
