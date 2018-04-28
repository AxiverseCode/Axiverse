using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Interface.Windows;

namespace Axiverse.Interface.Graphics
{
    public class WindowsPipeline : Pipeline
    {
        public Renderer Renderer { get; private set; }
        public Window Window { get; private set; }

        private Canvas canvas;

        public WindowsPipeline(Renderer renderer, Window window)
        {
            Renderer = renderer;
            Window = window;

            canvas = new Canvas(Window);
            canvas.Initialize(Renderer);
        }

        public override void Execute()
        {
            canvas.Draw();
        }

        public override void CreateBuffers()
        {
            canvas.InitializeFrames(Renderer.Device, Renderer.RenderTarget);
        }

        public override void ReleaseBuffers()
        {
            canvas.DisposeFrames();
        }

        public override void Dispose()
        {
            canvas.Dispose();
        }
    }
}
