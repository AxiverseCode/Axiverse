using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Entities;

using Buffer11 = SharpDX.Direct3D11.Buffer;

namespace Axiverse.Interface2.Engine
{
    public class SkyboxRenderer : Renderer
    {
        public Device device;
        public Shader shader;
        public Buffer11 bufferVs1;
        public VsData vsData1;
        public Texture2D skybox;

        public SkyboxRenderer(Device device)
        {
            this.device = device;
            bufferVs1 = device.CreateBuffer<VsData>();
            shader = new Shader(device, "../../Skybox.hlsl", vertexEntry: "VS", pixelEntry: "PS");
        }

        public override void Dispose()
        {
            shader.Dispose();
            bufferVs1.Dispose();
        }

        public override void Render(Renderable renderable, CompositingContext context)
        {
            base.Render(renderable, context);

            var entity = renderable.Entity;
            var transform = entity.Transform;
            var model = renderable.Model;
            var camera = context.Camera;

            shader.Apply(model.InputLayout);

            // Update general data.
            {
                vsData1.view = camera.View;
                vsData1.proj = camera.Projection;
                vsData1.bias = Vector3.One;
                device.UpdateData(bufferVs1, vsData1);
            }

            device.NativeDeviceContext.VertexShader.SetConstantBuffer(0, bufferVs1);
            device.NativeDeviceContext.PixelShader.SetShaderResource(0, skybox.NativeResourceView); // ao

            model.Draw(shader);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct VsData
        {
            public Matrix4 view;
            public Matrix4 proj;
            public Vector3 bias;
            public float unused;
        }
    }
}
