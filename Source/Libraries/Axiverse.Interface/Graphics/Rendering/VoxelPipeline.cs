using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D12;

namespace Axiverse.Interface.Graphics
{
    public class VoxelPipeline : Pipeline
    {
        public Renderer Renderer { get; private set; }
        public RenderTarget RenderTarget { get; set; }
        public SceneGraph Scene { get; private set; }

        public Mesh<PositionColorTextureMaterial> Mesh;
        public Texture Texture;
        private GraphicsCommandList commandList;
        public VoxelPipelineState PipelineState { get; set; }

        public VoxelPipeline(Renderer renderer, SceneGraph scene)
        {
            Renderer = renderer;
            RenderTarget = renderer.RenderTarget;
            Scene = scene;

            PipelineState = new VoxelPipelineState(Renderer);
            PipelineState.Load(@"Engine\Forward\Standard.hlsl", PositionColorTextureMaterial.Description);

            Texture = new Texture(Renderer);
            Texture.Load(@"Textures\Placeholder Grid.jpg");

            Mesh = Primitives<PositionColorTextureMaterial>.CreateCube(renderer);

        }

        public override void Execute()
        {
            if (commandList == null)
            {
                commandList = Renderer.Device.CreateCommandList(CommandListType.Direct, Renderer.CommandAllocator, null);
                commandList.Close();
            }

            commandList.Reset(Renderer.CommandAllocator, null);
            commandList.PipelineState = PipelineState.PipelineState;
            commandList.SetViewport(RenderTarget.Viewport);
            commandList.SetScissorRectangles(RenderTarget.ScissorRectangle);

            //// TODO: make this transition higher level.
            //// back buffer transition to render target
            //commandList.ResourceBarrierTransition(RenderTarget.RenderTargets[RenderTarget.FrameIndex], ResourceStates.Present, ResourceStates.RenderTarget);

            var renderTargetViewHandle = RenderTarget.RenderTargetViewDescriptorHeap.CPUDescriptorHandleForHeapStart +
                (RenderTarget.FrameIndex * RenderTarget.RenderTargetViewDescriptorSize);
            var depthTargetViewHandle = RenderTarget.DepthStencilViewDescriptorHeap.CPUDescriptorHandleForHeapStart;

            commandList.SetRenderTargets(renderTargetViewHandle, depthTargetViewHandle);
            //commandList.ClearRenderTargetView(renderTargetViewHandle, new Color4(0.0f, 0.0f, 0.0f, 1), 0, null);
            //commandList.ClearDepthStencilView(depthTargetViewHandle, ClearFlags.FlagsDepth, 1, 0);

            //// stuff here

            PipelineState.Apply(commandList);
            var viewProjection = Scene.Camera.ViewProjection;

            //for (int i = 0; i < Scene.Entities.Count; i++)
            //{
            //    var entity = Scene.Entities[i];
            //    var model = entity.GetComponent<ModelComponent>();
            //    if (model != null)
            //    {
            //        PipelineState.SetTexture(model.Texture);
            //        var matrix = Matrix.Transpose(entity.Transform.Transformation * viewProjection);
            //        PipelineState.SetTransform(matrix, i);
            //        PipelineState.ApplyConstantBuffer(commandList, i);
            //        model.Mesh.Draw(commandList);
            //    }
            //}
            PipelineState.SetTexture(Texture);
            PipelineState.SetTransform(Matrix.Transpose(viewProjection), 0);
            Mesh.Draw(commandList);

            // TODO: transition back if we're not going to do canvas stuff.
            // commandList.ResourceBarrierTransition(RenderTarget.RenderTargets[RenderTarget.FrameIndex], ResourceStates.RenderTarget, ResourceStates.Present);

            commandList.Close();
            Renderer.ExecuteCommandList(commandList);
        }

        public override void Dispose()
        {
            commandList.Dispose();
            commandList = null;
        }
    }
}
