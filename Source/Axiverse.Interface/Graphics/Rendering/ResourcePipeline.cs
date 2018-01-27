using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    /// <summary>
    /// Pipeline which performs resource uploading to the device.
    /// </summary>
    public class ResourcePipeline : Pipeline
    {
        public Renderer Renderer { get; private set; }

        public List<IGraphicsResource> Resources { get; } = new List<IGraphicsResource>();

        private GraphicsCommandList commandList;

        public ResourcePipeline(Renderer renderer)
        {
            Renderer = renderer;
        }

        public override void CreateBuffers()
        {
        }

        public override void Dispose()
        {
            commandList.Dispose();
            commandList = null;
        }

        public override void Execute()
        {
            if (Resources.Count == 0)
            {
                return;
            }

            if (commandList == null)
            {
                commandList = Renderer.Device.CreateCommandList(CommandListType.Direct, Renderer.CommandAllocator, null);
                commandList.Close();
            }

            commandList.Reset(Renderer.CommandAllocator, null);

            // prepare the resources
            Resources.ForEach(r => r.Prepare(commandList));

            // close, execute, and wait for the commands to execute
            commandList.Close();
            Renderer.CommandQueue.ExecuteCommandList(commandList);
            Renderer.SignalBlock();

            // collect any resources
            Resources.ForEach(r => r.Collect());
            Resources.Clear();
        }
    }
}
