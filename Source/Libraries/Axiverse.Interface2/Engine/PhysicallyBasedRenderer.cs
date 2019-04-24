using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Buffer11 = SharpDX.Direct3D11.Buffer;
using Axiverse.Interface2.Entites;

namespace Axiverse.Interface2.Engine
{
    public class PhysicallyBasedRenderer : Renderer
    {
        public Device device;
        public Shader shader;
        public Buffer11 bufferVs1;
        public Buffer11 bufferPs1;
        public Buffer11 bufferPs2;
        public VsData vsData1;
        public PsData psData1;
        public PsData2 psData2;
        public Texture2D skybox;

        public PhysicallyBasedRenderer(Device device)
        {
            this.device = device;
            bufferVs1 = device.CreateBuffer<VsData>();
            bufferPs1 = device.CreateBuffer<PsData>();
            bufferPs2 = device.CreateBuffer<PsData2>();
            shader = new Shader(device, "../../Pbr.hlsl", vertexEntry: "VS", pixelEntry: "PS");

            psData1.color = new Vector4(1, 0.7f, 0.5f, 1);
            psData1.color = new Vector4(1, 1f, 1f, 1);
            psData1.lightVector = new Vector3(0.5f, -0.5f, 0).Normal();
            psData1.lightVector = new Vector3(-1, 0, 0);
            psData1.intensity = 0.6f;
        }

        public override void Dispose()
        {
            bufferVs1.Dispose();
            bufferPs1.Dispose();
            bufferPs2.Dispose();

            shader.Dispose();
        }

        public override void Render(Renderable renderable, CompositingContext context)
        {
            var entity = renderable.Entity;
            var transform = entity.Transform;
            var model = renderable.Model;
            var material = model.Materials[0];
            var camera = context.Camera;

            shader.Apply(model.InputLayout);

            // Update general data.
            {
                vsData1.world = transform.Composite;
                vsData1.view = camera.View;
                vsData1.proj = camera.Projection;
                vsData1.camera = camera.Position;
                //vsData1.proj = world * view * proj;
                device.UpdateData(bufferVs1, vsData1);
            }

            // Update light data.
            {
                var lights = context.ClosestLights(transform.Origin);
                var light = lights[0];
                var lightPosition = light.Entity.Transform.Origin;
                var direction = transform.Origin - lightPosition;

                psData1.direction = direction.Normal();
                psData1.color = light.Color;
                psData1.lightVector = psData1.direction;
                psData1.position = lightPosition;
                device.UpdateData(bufferPs1, psData1);
            }

            device.NativeDeviceContext.VertexShader.SetConstantBuffer(0, bufferVs1);

            device.NativeDeviceContext.PixelShader.SetConstantBuffer(0, bufferPs1);
            device.NativeDeviceContext.PixelShader.SetConstantBuffer(1, bufferPs2);

            device.NativeDeviceContext.PixelShader.SetShaderResource(0, material.Albedo.NativeResourceView);
            device.NativeDeviceContext.PixelShader.SetShaderResource(1, material.Normal.NativeResourceView);
            device.NativeDeviceContext.PixelShader.SetShaderResource(2, material.Height?.NativeResourceView); // height
            device.NativeDeviceContext.PixelShader.SetShaderResource(3, material.Roughness?.NativeResourceView);
            device.NativeDeviceContext.PixelShader.SetShaderResource(4, material.Specular?.NativeResourceView);
            device.NativeDeviceContext.PixelShader.SetShaderResource(5, material.Alpha?.NativeResourceView); // alpha
            device.NativeDeviceContext.PixelShader.SetShaderResource(6, material.Occlusion?.NativeResourceView); // ao
            device.NativeDeviceContext.PixelShader.SetShaderResource(7, skybox.NativeResourceView); // ao

            model.Draw(shader);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct VsData
        {
            public Matrix4 proj;
            public Matrix4 view;
            public Matrix4 world;
            public Vector3 camera;
            public float unused;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PsData
        {
            public Vector4 color;
            public Vector3 lightVector;
            public float intensity;
            public Vector3 position;
            public float p0;
            public Vector3 direction;
            public float p1;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PsData2
        {
            Vector4 test;
        }
    }
}
