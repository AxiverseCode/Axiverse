using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX.Direct3D;

namespace Axiverse.Interface.Graphics
{
    using SharpDX.Direct3D12;


    public class Renderer : IDisposable
    {
        /// <summary>
        /// The pipelines which are part of the render process.
        /// </summary>
        public List<Pipeline> Pipelines { get; } = new List<Pipeline>();

        /// <summary>
        /// The resource pipeline for processing resource commands.
        /// </summary>
        public ResourcePipeline ResourcePipeline { get; private set; }

        public Device Device { get; private set; }
        public RenderTarget RenderTarget { get; private set; }

        public GraphicsCommandList commandList;
        public CommandQueue CommandQueue;
        public CommandAllocator CommandAllocator;

        public Fence Fence;
        public int FenceValue;
        public AutoResetEvent FenceEvent;
    
        public Renderer()
        {
            ResourcePipeline = new ResourcePipeline(this);
            Pipelines.Add(ResourcePipeline);
        }
        
        public void Initialize(RenderForm form)
        {
            //DebugInterface.Get().EnableDebugLayer();
            Device = new Device(null, FeatureLevel.Level_11_0);


            using (var factory = new Factory4())
            {
                // command queue
                var commandQueueDescription = new CommandQueueDescription(CommandListType.Direct);
                CommandQueue = Device.CreateCommandQueue(commandQueueDescription);

                // command allocator
                CommandAllocator = Device.CreateCommandAllocator(CommandListType.Direct);

                // fence
                Fence = Device.CreateFence(0, FenceFlags.None);
                FenceValue = 1;
                FenceEvent = new AutoResetEvent(false);
            }

            RenderTarget = new RenderTarget(this);
            RenderTarget.Initialize(form);
        }

        public void Execute()
        {
            if (Device == null)
            {
                throw new InvalidOperationException("Renderer must be initialized before execution.");
            }

            if (RenderTarget.NeedsBufferResize)
            {
                Pipelines.ForEach(pipeline => pipeline.ReleaseBuffers());
                RenderTarget.ResizeBuffers();
                Pipelines.ForEach(pipeline => pipeline.CreateBuffers());
            }

            CommandAllocator.Reset();

            Pipelines.ForEach(pipeline => pipeline.Execute());

            RenderTarget.Present();
            SignalBlock();
        }

        public void ExecuteCommandList(CommandList commandList)
        {
            CommandQueue.ExecuteCommandList(commandList);
        }

        public void SignalBlock()
        {
            var fenceSignal = FenceValue;

            CommandQueue.Signal(Fence, fenceSignal);
            FenceValue++;

            if (Fence.CompletedValue < fenceSignal)
            {
                Fence.SetEventOnCompletion(fenceSignal, FenceEvent.SafeWaitHandle.DangerousGetHandle());
                FenceEvent.WaitOne();
            }
        }

        public void Dispose()
        {
            Pipelines.ForEach(p => p.Dispose());

            commandList.Dispose();
            RenderTarget.Dispose();

            Fence.Dispose();
            CommandAllocator.Dispose();
            CommandQueue.Dispose();
        }
    }
}
