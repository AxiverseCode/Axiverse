using Axiverse.Interface.Scenes;
using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Rendering.Compositing
{
    public class ForwardRenderer : Renderer
    {
        private Processor<RenderableComponent> renderableProcessor = new Processor<RenderableComponent>();

        public ForwardRenderer()
        {
            Scene.Processors.Add(renderableProcessor);
        }

        public override void Collect(RenderContext context)
        {
            // renderable processor does our work here.
        }

        public override void Prepare(RenderContext context)
        {
            // prepare the constant buffers for the selected effects
            //constantBuffer.Write(transforms);
        }

        public override void Render(RenderContext context)
        {
            // render each of the models
            //commandList.SetRootSignature(pipelineStateDescription.RootSignature);
            //commandList.PipelineState = pipelineState;

            //for (int i = 0; i < meshes.Length; i++)
            //{
            //    transforms[i].WorldViewProjection = (Matrix4)meshes[i].Bindings[Key.From<Matrix4>()];

            //    commandList.SetDescriptors(descriptorSets[i]);

            //    Draw(commandList, meshes[i].Draw);
            //}
        }
    }
}
