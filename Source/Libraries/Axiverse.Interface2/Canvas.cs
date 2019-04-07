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

namespace Axiverse.Interface2
{
    public class Canvas : IDisposable
    {
        public Device Device { get; }

        public RenderTarget RenderTarget { get; set; }
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
        }


        /// <summary>
        /// Begin a 2D drawing session
        /// </summary>
        public void Begin()
        {
            RenderTarget.BeginDraw();
        }

        /// <summary>
        /// End drawing session
        /// </summary>
        public void End()
        {
            RenderTarget.EndDraw();
        }

        /// <summary>
        /// Update all resources
        /// </summary>
        /// <param name="backBuffer">BackBuffer</param>
        internal void UpdateResources(Texture2D11 backBuffer)
        {

            var factory2D = new SharpDX.Direct2D1.Factory();
            var surface2D = backBuffer.QueryInterface<Surface>();
            RenderTarget = new RenderTarget(factory2D, surface2D, 
                new RenderTargetProperties(new PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied)));
            surface2D.Dispose();
            factory2D.Dispose();

            InitializeFont();
        }

        private void InitializeFont()
        {
            var factoryWrite = new SharpDX.DirectWrite.Factory();
            TextFormat = new TextFormat(factoryWrite, fontName, fontSize) {
                TextAlignment = TextAlignment.Leading,
                ParagraphAlignment = ParagraphAlignment.Near
            };
            Brush = new SolidColorBrush(RenderTarget, fontColor);
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
            RenderTarget.DrawText(text, TextFormat, new RawRectangleF(x, y, width, height), Brush);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            TextFormat?.Dispose();
            TextFormat = null;
            Brush?.Dispose();
            Brush = null;
            RenderTarget?.Dispose();
            RenderTarget = null;
        }
    }
}
