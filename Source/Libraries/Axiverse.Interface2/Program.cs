using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface.Windows;
using Axiverse.Interface2.Models;
using Axiverse.Physics;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;

using Buffer11 = SharpDX.Direct3D11.Buffer;

namespace Axiverse.Interface2
{
    using Vector3 = SharpDX.Vector3;
    using Color = SharpDX.Color;

    class Program
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Constants
        {
            public Matrix worldViewProj;
            public Matrix worldView;
            public Matrix proj;
            public Vector4 color;
        }

        public static Stopwatch watch = new Stopwatch();

        static Matrix4 Convert (Matrix m)
        {
            return new Matrix4(
                m.M11, m.M12, m.M13, m.M14,
                m.M21, m.M22, m.M23, m.M24,
                m.M31, m.M32, m.M33, m.M34,
                m.M41, m.M42, m.M43, m.M44
                );
        }

        static void Main(string[] args)
        {
            Queue<float> frametime = new Queue<float>();
            List<Entity> entities = new List<Entity>();

            using (var form = new RenderForm() { ClientSize = new System.Drawing.Size(500,500)})
            using (var device = new Device(form))
            {
                form.Text = "Axiverse Engine";
                var overlay = new Interface.Chrome(form);
                var menu = new Interface.Menu()
                {
                    Position = new Vector2(),
                    Backcolor = new Color(0.2f, 0.2f, 0.2f, 0.8f),
                    Forecolor = Color.White,
                    Size = new Vector2(9000, 40)
                };
                menu.Items.Add(new Interface.MenuItem("File"));
                menu.Items[0].Children.Add(new Interface.MenuItem("New"));
                menu.Items[0].Children.Add(new Interface.MenuItem("Open"));
                menu.Items[0].Children.Add(new Interface.MenuItem("Save"));
                menu.Items[0].Children.Add(new Interface.MenuItem("Exit"));
                menu.Items.Add(new Interface.MenuItem("Edit"));
                menu.Items[1].Children.Add(new Interface.MenuItem("Cut"));
                menu.Items[1].Children.Add(new Interface.MenuItem("Copy"));
                menu.Items[1].Children.Add(new Interface.MenuItem("Paste"));
                menu.Items.Add(new Interface.MenuItem("View"));
                menu.Items.Add(new Interface.MenuItem("Window"));
                menu.Items.Add(new Interface.MenuItem("Help"));
                menu.Items[4].Children.Add(new Interface.MenuItem("About"));

                overlay.Controls.Add(menu);
                overlay.Controls.Add(new Interface.Slider()
                {
                    Position = new Vector2(10, 300),
                    Size = new Vector2(200, 40),
                    Text = "Hello World",
                    Backcolor = new Color(0.1f, 0.1f, 0.1f),
                    Forecolor = new Color(1f),
                });

                Vector2 mouse = new Vector2();
                Mathematics.Ray3 ray = new Mathematics.Ray3();
                Mathematics.Viewport vp = new Mathematics.Viewport()
                {
                    Width = form.ClientRectangle.Width,
                    Height = form.ClientRectangle.Height,
                    Near = 1,
                    Far = 1000,
                };


                Matrix4 wvp = new Matrix4();
                float ratio = (float)form.ClientRectangle.Width / form.ClientRectangle.Height;
                Matrix projection = Matrix.PerspectiveFovLH(3.14F / 3.0F, ratio, 1, 1000);
                Matrix view;

                TrackballControl control = new TrackballControl();
                control.CameraPosition = new Axiverse.Vector3(0, 0, -50);
                control.ZoomEnabled = true;
                control.RotateEnabled = true;
                control.Screen = new Rectangle(0, 0, vp.Width, vp.Height);
                control.Enabled = true;
                control.Up = new Axiverse.Vector3(0, 1, 0);

                form.Resize += (s, e) =>
                {
                    vp.Width = form.ClientRectangle.Width;
                    vp.Height = form.ClientRectangle.Height;
                    ratio = (float)form.ClientRectangle.Width / form.ClientRectangle.Height;
                    projection = Matrix.PerspectiveFovLH(3.14F / 3.0F, ratio, 1, 1000);
                    control.Screen = new Rectangle(0, 0, vp.Width, vp.Height);
                };
                form.MouseDown += (s, e) =>
                {
                    TrackballControl.State state;
                    switch (e.Button)
                    {
                        case System.Windows.Forms.MouseButtons.Left:
                            state = TrackballControl.State.Rotate;
                            break;
                        case System.Windows.Forms.MouseButtons.Right:
                            state = TrackballControl.State.Zoom;
                            break;
                        default:
                            state = TrackballControl.State.None;
                            break;
                    }
                    control.OnMouseDown(state, new Vector2(vp.Width - e.X, e.Y));
                };
                form.MouseUp += (s, e) =>
                {
                    control.OnMouseUp();
                };
                form.MouseWheel += (s, e) =>
                {
                    control.OnMouseWheel(e.Delta / 6f);
                };
                form.MouseMove += (s, e) =>
                {
                    mouse = new Vector2(e.X, e.Y);
                    control.OnMouseMove(new Vector2(vp.Width - e.X, e.Y));
                };



                BlendStateDescription description = BlendStateDescription.Default();
                description.RenderTarget[0].IsBlendEnabled = true;
                description.RenderTarget[0].BlendOperation = BlendOperation.Add;
                description.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
                description.RenderTarget[0].DestinationAlphaBlend = BlendOption.One;
                description.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
                description.RenderTarget[0].DestinationBlend = BlendOption.SourceAlpha;
                var blendState = new BlendState(device.NativeDevice, description);


                DepthStencilStateDescription dsdesc = DepthStencilStateDescription.Default();
                dsdesc.DepthComparison = Comparison.Less;
                dsdesc.DepthWriteMask = DepthWriteMask.Zero;
                dsdesc.IsDepthEnabled = true;
                var depthStencilState = new DepthStencilState(device.NativeDevice, dsdesc);

                Mesh axis = Mesh.CreateAxis(device);
                var mmesh = Mathematics.Geometry.WavefrontObj.Load("../../Model.obj");
                Model model = Model.FromMesh(device, mmesh);
                mmesh = null;

                Mesh mesh = Mesh.LoadMesh(device, "../../Model.obj");
                mesh.Transform = Matrix.Scaling(50);
                Mesh missile = Mesh.LoadMesh(device, "../../Missile.obj");
                missile.Transform = Matrix.Translation(0, -1, 0) * Matrix.Scaling(0.5f, 4, 0.5f);
                Mesh rayMesh = Mesh.CreateDynamic(device, 20);
                Mesh sphereMesh = Mesh.CreateSphere(device);
                sphereMesh.Transform = Matrix.Translation(10, 0, 0);

                Shader shader = new Shader(device, "../../Shader.hlsl", "VS", "PS", Mesh.ColoredTexturedVertex.Elements);
                Buffer11 buffer = device.CreateBuffer<Constants>();
                Image2D image = Image2D.LoadFromFile("../../Weapon1.png", device.Canvas);
                ShaderResourceView texture = Texture.CreateTextureFromBitmap(device, "../../Texture.png");
                ShaderResourceView particleTex = Texture.CreateTextureFromBitmap(device, "../../Particle.png");

                Shader particleShader = new Shader(device, "../../Particles.hlsl", "VS", "PS", Mesh.ColoredTexturedVertex.Elements, "GS");
                Constants constants = new Constants();
                var pbr = new Pbr(device);

                Entity shipEntity;
                entities.Add(shipEntity = new Entity()
                {
                    Mesh = mesh,
                    Model = model,
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
                    frametime.Enqueue(dt);

                    device.Start();

                    //clear color
                    //device.Clear(Color.DarkGray);
                    device.Clear(Color.Black);

                    //Set matrices
                    control.Update();
                    Matrix world = Matrix.Identity;
                    view = Matrix.LookAtLH(new Vector3(control.CameraPosition.X, control.CameraPosition.Y, control.CameraPosition.Z),
                        new Vector3(control.Up.X, control.Up.Y, control.Up.Z), Vector3.UnitY);
                    Matrix worldViewProjection = world * view * projection;
                    wvp = Convert(worldViewProjection);
                    ray = Mathematics.Ray3.FromScreen(mouse.X, mouse.Y, vp, wvp);

                    //update constant buffer

                    //pass constant buffer to shader
                    device.NativeDeviceContext.VertexShader.SetConstantBuffer(0, buffer);
                    device.NativeDeviceContext.GeometryShader.SetConstantBuffer(0, buffer);
                    device.NativeDeviceContext.PixelShader.SetShaderResource(0, texture);

                    //apply shader
                    //shader.Apply();

                    pbr.Setup(Convert(sphereMesh.Transform * world), Convert(view), Convert(projection), control.CameraPosition, current);
                    sphereMesh.Draw();

                    //draw mesh
                    foreach (var entity in entities)
                    {
                        entity.Update(dt / 10f);

                        pbr.Setup(Convert(entity.Mesh.Transform * entity.Transform * world), Convert(view), Convert(projection), control.CameraPosition, current);

                        if (ray.Distance(entity.Body.LinearPosition) < 1)
                        {
                            constants.color = Vector4.One;
                        }
                        constants.worldViewProj = entity.Mesh.Transform * entity.Transform * worldViewProjection;
                        device.UpdateData(buffer, constants);

                        if (entity.Model == null)
                        {
                            entity.Mesh.Draw();
                        }
                        else
                        {
                            entity.Model.DrawRaw();
                        }

                        shader.Apply();
                        device.NativeDeviceContext.VertexShader.SetConstantBuffer(0, buffer);
                        device.NativeDeviceContext.GeometryShader.SetConstantBuffer(0, buffer);
                        device.NativeDeviceContext.PixelShader.SetShaderResource(0, texture);

                        constants.color = Vector4.Zero;
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

                    for (int i = 0; i < rayMesh.Dynamic.Length; i++)
                    {
                        rayMesh.Dynamic[i].Color = Vector4.One;
                        rayMesh.Dynamic[i].Texture = new Vector2(0.01f, 0.01f);
                        rayMesh.Dynamic[i].Position = ray.Origin + ray.Direction * (i / 5f);
                    }
                    rayMesh.UpdateDynamic();
                    rayMesh.DrawPoints();

                    device.SetBlendState(device.blendState);
                    device.SetDepthStencil(device.depthStencilState);


                    device.Canvas.Begin();
                    {
                        while (frametime.Count > 20)
                        {
                            frametime.Dequeue();
                        }

                        device.Canvas.DrawString(1 / frametime.Average() + " fps ", 10, 60);


                        var target = entities[1];
                        var relative = RelativeFrame.FromBody(target.Body);
                        device.Canvas.DrawString("Oriented Velocity: " + relative.LinearVelocity.ToString(2), 10, 130);
                        device.Canvas.DrawString("Local Angular:" + relative.AngularVelocity.ToString(2), 10, 150);
                        device.Canvas.DrawString("Position:" + target.Body.LinearPosition.ToString(2), 10, 170);
                        device.Canvas.DrawString("Mouse:" + mouse.ToString() + " " + ray.ToString(), 10, 200);
                        device.Canvas.DrawImage(image, new Vector2(10, 300));

                        overlay.Update(dt);
                        overlay.Draw(device.Canvas);

                        device.Canvas.End();
                    }

                    device.Present();
                    previous = current;
                });

                foreach (var entity in entities)
                {
                    entity.Dispose();
                }

                texture.Dispose();
                particleTex.Dispose();
                particleShader.Dispose();
                shader.Dispose();
                image.nativeBitmap.Dispose();
                buffer.Dispose();
                mesh.Dispose();
                missile.Dispose();
            }
        }
    }
}
