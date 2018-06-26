using Axiverse.Injection;
using Axiverse.Interface.Graphics;
using Axiverse.Interface.Graphics.Generic;
using Axiverse.Interface.Graphics.Shaders;
using Axiverse.Interface.Scenes;
using Axiverse.Simulation;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Rendering.Compositing
{
    public class ForwardRenderer : Renderer
    {
        private GeometryShader geometryShader;
        private PipelineState pipelineState;
        private PipelineStateDescription pipelineStateDescription;

        private const int batchSize = 16;
        private int perObjectSize = Utilities.SizeOf<GeometryShader.PerObject>();
        private DescriptorSet[] descriptorSets = new DescriptorSet[batchSize];
        private GeometryShader.PerObject[] constants = new GeometryShader.PerObject[batchSize];

        private DisposableObjectPool<GraphicsBuffer> constantBufferPool;
        private Queue<GraphicsBuffer> constantBufferQueue = new Queue<GraphicsBuffer>();

        private DisposableObjectPool<DescriptorSet> descriptorPool;
        private Queue<DescriptorSet> descriptorQueue = new Queue<DescriptorSet>();

        private Processor<RenderableComponent, TransformComponent> processor;
        private SamplerState samplerState;

        public ForwardRenderer(GraphicsDevice device)
        {
            //Scene.Processors.Add(renderableProcessor);
            samplerState = SamplerState.Create(device, null);

            descriptorPool = new DisposableObjectPool<DescriptorSet>(() => new DescriptorSet(device, geometryShader.Layout));
            constantBufferPool = new DisposableObjectPool<GraphicsBuffer>(() => GraphicsBuffer.Create(device, constants, false));

            geometryShader = new GeometryShader(device);
            geometryShader.Initialize();
            pipelineStateDescription = new PipelineStateDescription()
            {
                InputLayout = PositionColorTexture.Layout,
                RootSignature = geometryShader.RootSignature,
                VertexShader = geometryShader.VertexShader,
                PixelShader = geometryShader.PixelShader,
            };
            pipelineState = PipelineState.Create(device, pipelineStateDescription);
        }

        public override void Collect(RenderContext context)
        {
            if (processor == null)
            {
                processor = new Processor<RenderableComponent, TransformComponent>();
                context.Scene.Add(processor);
            }
            // renderable processor does our work here.
        }

        public override void Prepare(RenderContext context)
        {
            // prepare the constant buffers for the selected effects
            //constantBuffer.Write(transforms);
        }

        public override void Render(RenderContext context)
        {
            context.CommandList.SetRootSignature(pipelineStateDescription.RootSignature);
            context.CommandList.PipelineState = pipelineState;

            var commandList = context.CommandList;
            
            var constantBuffer = constantBufferPool.Take();
            constantBufferQueue.Enqueue(constantBuffer);
            var offset = 0;

            foreach (var entity in processor.Entities.Values)
            {
                var transform = entity.Components.Get<TransformComponent>();
                var renderable = entity.Components.Get<RenderableComponent>();

                var descriptorSet = descriptorPool.Take();
                descriptorQueue.Enqueue(descriptorSet);
                descriptorSet.SetShaderResourceView(1, renderable.Mesh.Bindings.Get<Texture>());
                descriptorSet.SetSamplerState(0, samplerState);
                descriptorSet.SetConstantBuffer(0, constantBuffer, offset * perObjectSize, perObjectSize);

                constants[offset].WorldViewProjection = Matrix4.Transpose(transform.GlobalTransform * context.Camera.View * context.Camera.Projection);
                constants[offset].Color = Vector4.One;

                commandList.SetDescriptors(descriptorSet);

                var meshDraw = renderable.Mesh.Draw;
                commandList.SetIndexBuffer(meshDraw.IndexBuffer);
                for (int i = 0; i < meshDraw.VertexBuffers.Length; i++)
                {
                    commandList.SetVertexBuffer(meshDraw.VertexBuffers[i], i);
                }

                commandList.Draw(meshDraw.Count, meshDraw.IndexBuffer != null);

                offset++;
                if (offset > batchSize)
                {
                    constantBuffer.Write(constants);
                    constantBuffer = constantBufferPool.Take();
                    offset = 0;
                }
            }

            if (offset != 0)
            {
                constantBuffer.Write(constants);
            }
        }

        public override void Release(RenderContext context)
        {
            base.Release(context);

            descriptorPool.AddAll(descriptorQueue);
            descriptorQueue.Clear();

            constantBufferPool.AddAll(constantBufferQueue);
            constantBufferQueue.Clear();

        }
    }
}
