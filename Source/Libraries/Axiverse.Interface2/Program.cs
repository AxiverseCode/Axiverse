using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Physics;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;

using Buffer11 = SharpDX.Direct3D11.Buffer;

namespace Axiverse.Interface2
{
    using Vector3 = SharpDX.Vector3;

    class Program
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Constants
        {
            public Matrix worldViewProj;
            public Matrix worldView;
            public Matrix proj;
        }

        public static Stopwatch watch = new Stopwatch();
        static int frame = 0;

        static void Main(string[] args)
        {
            List<Entity> entities = new List<Entity>();

            using (var form = new RenderForm() { ClientSize = new System.Drawing.Size(500,500)})
            using (var device = new Device(form))
            {

                BlendStateDescription description = BlendStateDescription.Default();
                description.RenderTarget[0].IsBlendEnabled = true;
                description.RenderTarget[0].BlendOperation = BlendOperation.Add;
                description.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
                description.RenderTarget[0].DestinationAlphaBlend = BlendOption.One;
                description.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
                description.RenderTarget[0].DestinationBlend = BlendOption.SourceAlpha;
                var blendState = new BlendState(device.NativeDevice, description);


                DepthStencilStateDescription dsdesc = DepthStencilStateDescription.Default();
                dsdesc.DepthComparison = Comparison.Always;
                dsdesc.DepthWriteMask = DepthWriteMask.Zero;
                dsdesc.IsDepthEnabled = true;
                var depthStencilState = new DepthStencilState(device.NativeDevice, dsdesc);

                Mesh axis = Mesh.CreateAxis(device);

                Mesh mesh = Mesh.LoadMesh(device, "../../Model.obj");
                mesh.Transform = Matrix.Scaling(50);
                Mesh missile = Mesh.LoadMesh(device, "../../Missile.obj");
                missile.Transform = Matrix.Translation(0, -1, 0) * Matrix.Scaling(0.5f, 4, 0.5f);

                Shader shader = new Shader(device, "../../Shader.hlsl", "VS", "PS", Mesh.ColoredTexturedVertex.Elements);
                Buffer11 buffer = device.CreateBuffer<Constants>();
                Image2D image = Image2D.LoadFromFile("../../Weapon1.png", device.Canvas);
                ShaderResourceView texture = Texture.CreateTextureFromBitmap(device, "../../Texture.png");
                ShaderResourceView particleTex = Texture.CreateTextureFromBitmap(device, "../../Particle.png");

                Shader particleShader = new Shader(device, "../../Particles.hlsl", "VS", "PS", Mesh.ColoredTexturedVertex.Elements, "GS");
                Constants constants = new Constants();

                Entity shipEntity;
                entities.Add(shipEntity = new Entity() {
                    Mesh = mesh,
                });
                shipEntity.Body.LinearPosition = new Axiverse.Vector3(0, 10, 0);

                
                for (int i = 0; i < 10; i++)
                {
                    HomingEntity missileEntity;
                    entities.Add(missileEntity = new HomingEntity(device)
                    {
                        Mesh = missile,
                    });
                    missileEntity.Body.LinearPosition = new Axiverse.Vector3(0, 1, 0);
                    missileEntity.Body.AngularPosition = Functions.Random.NextQuaternion();
                }

                watch.Start();
                float previous = watch.ElapsedMilliseconds / 1000f;
                RenderLoop.Run(form, () =>
                {
                    float current = watch.ElapsedMilliseconds / 1000f;
                    float dt = current - previous;

                    device.Start();

                    //clear color
                    //device.Clear(Color.DarkGray);
                    device.Clear(Color.Black);

                    //Set matrices
                    float ratio = (float)form.ClientRectangle.Width / form.ClientRectangle.Height;
                    Matrix projection = Matrix.PerspectiveFovLH(3.14F / 3.0F, ratio, 1, 1000);
                    Matrix view = Matrix.LookAtLH(new Vector3(0, 10, -50), new Vector3(), Vector3.UnitY);
                    Matrix world = Matrix.RotationY(watch.ElapsedMilliseconds / 1000.0F / 10);
                    //world = Matrix.Identity;
                    Matrix worldViewProjection = world * view * projection;

                    //update constant buffer

                    //pass constant buffer to shader
                    device.NativeDeviceContext.VertexShader.SetConstantBuffer(0, buffer);
                    device.NativeDeviceContext.GeometryShader.SetConstantBuffer(0, buffer);
                    device.NativeDeviceContext.PixelShader.SetShaderResource(0, texture);

                    //apply shader
                    shader.Apply();

                    //draw mesh
                    foreach (var entity in entities)
                    {
                        entity.Update(dt / 10f);

                        constants.worldViewProj = entity.Mesh.Transform * entity.Transform * worldViewProjection;
                        device.UpdateData(buffer, constants);

                        entity.Mesh.Draw();

                        constants.worldViewProj = entity.Transform * worldViewProjection;
                        device.UpdateData(buffer, constants);
                        axis.DrawLines();
                    }

                    constants.worldViewProj = worldViewProjection;
                    constants.worldView = world * view;
                    constants.proj = projection;
                    device.UpdateData(buffer, constants);
                    axis.DrawLines();

                    device.NativeDeviceContext.PixelShader.SetShaderResource(0, particleTex);
                    device.SetBlendState(blendState);
                    device.SetDepthStencil(depthStencilState);
                    particleShader.Apply();
                    foreach (var entity in entities)
                    {
                        if (entity is HomingEntity homing)
                        {
                            homing.ParticleMesh.DrawPoints();
                        }
                    }
                    device.SetBlendState(device.blendState);
                    device.SetDepthStencil(device.depthStencilState);


                    device.Canvas.Begin();
                    {
                        frame++;
                        var seconds = watch.ElapsedMilliseconds / 1000f;

                        device.Canvas.DrawString(1 / (seconds / frame) + " fps ", 10, 10);


                        var target = entities[1];
                        var relative = RelativeFrame.FromBody(target.Body);
                        device.Canvas.DrawString("Oriented Velocity: " + relative.LinearVelocity.ToString(2), 10, 30);
                        device.Canvas.DrawString("Local Angular:" + relative.AngularVelocity.ToString(2), 10, 50);
                        device.Canvas.DrawString("Position:" + target.Body.LinearPosition.ToString(2), 10, 70);
                        device.Canvas.DrawImage(image, new Vector2(10, 100));

                        device.Canvas.End();
                    }

                    device.Present();
                    previous = current;
                });
            }
        }
    }
}
