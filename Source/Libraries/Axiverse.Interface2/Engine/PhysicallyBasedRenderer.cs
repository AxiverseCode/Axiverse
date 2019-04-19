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

        public PhysicallyBasedRenderer(Device device)
        {
            this.device = device;
            bufferVs1 = device.CreateBuffer<VsData>();
            bufferPs1 = device.CreateBuffer<PsData>();
            bufferPs2 = device.CreateBuffer<PsData2>();
            shader = new Shader(device, "../../Pbr.hlsl", "VS", "PS", Mesh.ColoredTexturedVertex.Elements);

            psData1.color = new Vector4(1, 0.7f, 0.5f, 1);
            psData1.color = new Vector4(1, 1f, 1f, 1);
            psData1.lightVector = new Vector3(0.5f, -0.5f, 0).Normal();
            psData1.lightVector = new Vector3(-1, 0, 0);
            psData1.intensity = 0.6f;
        }

        public override void Render(Renderable renderable, CompositingContext context)
        {
            var entity = renderable.Entity;
            var transform = entity.Transform;
            var model = renderable.Model;
            var material = model.Materials[0];
            var camera = context.Camera;

            shader.Apply();

            vsData1.world = transform.Composite;
            vsData1.view = camera.View;
            vsData1.proj = camera.Projection;
            vsData1.camera = camera.Position;
            //vsData1.proj = world * view * proj;
            device.UpdateData(bufferVs1, vsData1);

            psData1.direction = new Vector3(0, 1, 1).Normal();
            //psData1.lightVector = new Vector3(Functions.Sin(t), -0.1f, Functions.Cos(t)).Normal();
            //psData1.lightVector = psData1.direction;
            device.UpdateData(bufferPs1, psData1);

            device.NativeDeviceContext.VertexShader.SetConstantBuffer(0, bufferVs1);

            device.NativeDeviceContext.PixelShader.SetConstantBuffer(0, bufferPs1);
            device.NativeDeviceContext.PixelShader.SetConstantBuffer(1, bufferPs2);

            device.NativeDeviceContext.PixelShader.SetShaderResource(0, material.Albedo.NativeResourceView);
            device.NativeDeviceContext.PixelShader.SetShaderResource(1, material.Normal.NativeResourceView);
            device.NativeDeviceContext.PixelShader.SetShaderResource(2, null); // height
            device.NativeDeviceContext.PixelShader.SetShaderResource(3, material.Roughness.NativeResourceView);
            device.NativeDeviceContext.PixelShader.SetShaderResource(4, material.Specular.NativeResourceView);
            device.NativeDeviceContext.PixelShader.SetShaderResource(5, null); // alpha
            device.NativeDeviceContext.PixelShader.SetShaderResource(6, null); // ao
        }

        public void Setup(Matrix4 world, Matrix4 view, Matrix4 proj, Vector3 camera, float t)
        {
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
