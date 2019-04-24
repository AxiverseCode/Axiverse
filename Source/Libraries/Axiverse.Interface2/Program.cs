using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface.Windows;
using Axiverse.Interface2.Engine;
using Axiverse.Interface2.Entites;
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

            using (var form = new RenderForm() { ClientSize = new System.Drawing.Size(800,800)})
            using (var device = new Device(form))
            using (var renderer = new PhysicallyBasedRenderer(device))
            using (var skyRenderer = new SkyboxRenderer(device))
            {
                form.Text = "Axiverse Engine";
                var chrome = new Interface.Chrome(form);
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

                chrome.Controls.Add(menu);
                chrome.Controls.Add(new Interface.Slider()
                {
                    Position = new Vector2(10, 400),
                    Size = new Vector2(200, 40),
                    Text = "Hello World",
                    Backcolor = new Color(0.1f, 0.1f, 0.1f),
                    Forecolor = new Color(1f),
                });

                var tree = new Interface.Tree()
                {
                    Position = new Vector2(200, 80),
                    Size = new Vector2(160, 300),
                    Backcolor = new Color(0.1f, 0.1f, 0.1f),

                };
                tree.Items.Add(new Interface.TreeItem("Entity 0"));
                tree.Items.Add(new Interface.TreeItem("Entity 1"));
                tree.Items.Add(new Interface.TreeItem("Entity 2"));
                tree.Items[1].Children.Add(new Interface.TreeItem("Entity 1.a"));
                tree.Items[1].Children.Add(new Interface.TreeItem("Entity 1.b"));
                tree.Items[1].Children[1].Children.Add(new Interface.TreeItem("Entity 1.b.i"));
                tree.Items[1].Children[1].Children.Add(new Interface.TreeItem("Entity 1.b.ii"));
                tree.CalculateMetrics();
                chrome.Controls.Add(tree);

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


                var skyBox = Texture2D.FromFile(device,
                    "../../skybox/right.jpg",
                    "../../skybox/left.jpg",
                    "../../skybox/top.jpg",
                    "../../skybox/bottom.jpg",
                    "../../skybox/front.jpg",
                    "../../skybox/back.jpg");
                renderer.skybox = skyBox;
                skyRenderer.skybox = skyBox;

                var compositor = new Compositor();
                var scene = new Scene();


                var material = new Material()
                {
                    Albedo = Texture2D.FromFile(device, "../../pbr/albedo.jpg"),
                    Normal = Texture2D.FromFile(device, "../../pbr/normal.jpg"),
                    Roughness = Texture2D.FromFile(device, "../../pbr/roughness.jpg"),
                    Height = Texture2D.FromFile(device, "../../pbr/height.jpg"),
                    Specular = Texture2D.FromFile(device, "../../pbr/metallic.jpg"),
                    Alpha = Texture2D.FromFile(device, "../../pbr/alpha.jpg"),
                    Occlusion = Texture2D.FromFile(device, "../../pbr/ambientocclusion.jpg"),
                };
                var shipModel = Model.FromMesh(device, Mathematics.Geometry.WavefrontObj.Load("../../Model.obj"));
                shipModel.Materials.Add(material);
                var boxModel = Model.FromMesh(device, Mathematics.Geometry.Mesh.CreateCube().Invert().CalculateNormals());
                var box2Model = Model.FromMesh(device, Mathematics.Geometry.Mesh.CreateCube().CalculateNormals());


                var material2 = new Material
                {
                    Albedo = Texture2D.FromFile(device, "../../pbr/grimy-metal-albedo.png"),
                    Normal = Texture2D.FromFile(device, "../../pbr/grimy-metal-normal-ogl.png"),
                    Roughness = Texture2D.FromFile(device, "../../pbr/grimy-metal-metalness.png"),
                    Specular = Texture2D.FromFile(device, "../../pbr/grimy-metal-metalness.png"),
                    Alpha = null,
                    Occlusion = null,
                };
                var sphereModel = Model.FromMesh(device, Mathematics.Geometry.Mesh.CreateSphere(10, 20).CalculateNormals());
                sphereModel.Materials.Add(material2);
                box2Model.Materials.Add(material2);

                Camera camera;
                {
                    var entity = new Entity("Camera");
                    camera = new Camera()
                    {
                        View = Matrix4.Identity,
                        Projection = Convert(projection),
                    };
                    entity.Add(camera);
                    var renderable = new Renderable()
                    {
                        Model = boxModel,
                        //Model = shipModel,
                        Renderer = skyRenderer,
                    };
                    entity.Add(renderable);
                    //entity.Transform.Scaling = new Axiverse.Vector3(55);
                    scene.Entities.Add(entity);
                }

                {
                    var entity = new Entity("Box");
                    var renderable = new Renderable()
                    {
                        Model = box2Model, //shipModel,
                        Renderer = renderer,
                    };
                    entity.Add(renderable);
                    //entity.Transform.Scaling = new Axiverse.Vector3(55);
                    scene.Entities.Add(entity);
                }

                {
                    var entity = new Entity("Ship 1");
                    var renderable = new Renderable()
                    {
                        Model = shipModel,
                        Renderer = renderer,
                    };
                    entity.Add(renderable);
                    entity.Transform.Scaling = new Axiverse.Vector3(40);
                    entity.Transform.Translation = new Axiverse.Vector3(-30, 0, 0);
                    scene.Entities.Add(entity);
                }

                {
                    var entity = new Entity("Ship 2");
                    var renderable = new Renderable()
                    {
                        Model = shipModel,
                        Renderer = renderer,
                    };
                    entity.Add(renderable);
                    entity.Transform.Scaling = new Axiverse.Vector3(40);
                    entity.Transform.Translation = new Axiverse.Vector3(0, -30, 0);
                    scene.Entities.Add(entity);
                }

                Entity lightEntity;
                {
                    var entity = new Entity("Point Light");
                    var light = new Light()
                    {
                        Color = new Vector4(0.6f, 0.9f, 0.8f, 1),
                        Intensity = 1,
                    };
                    entity.Add(light);
                    entity.Transform.Translation = new Axiverse.Vector3(4, 5, 6);
                    scene.Entities.Add(entity);
                    lightEntity = entity;
                }


                var entityTree = new Interface.Custom.EntityComponentTree(scene)
                {
                    Position = new Vector2(10, 500),
                    Size = new Vector2(160, 300),
                    Backcolor = new Color(0.1f, 0.1f, 0.1f),
                };
                entityTree.CalculateMetrics();
                chrome.Controls.Add(entityTree);


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
                    camera.Projection = Convert(projection);
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


                watch.Start();
                float previous = watch.ElapsedMilliseconds / 1000f;
                RenderLoop.Run(form, () =>
                {
                    float current = watch.ElapsedMilliseconds / 1000f;
                    float dt = current - previous;
                    frametime.Enqueue(dt);
                    control.Update();

                    device.Start();
                    device.Clear(Color.Black);

                    Matrix world = Matrix.Identity;
                    view = Matrix.LookAtLH(new Vector3(control.CameraPosition.X, control.CameraPosition.Y, control.CameraPosition.Z),
                        new Vector3(control.Up.X, control.Up.Y, control.Up.Z), Vector3.UnitY);
                    Matrix worldViewProjection = world * view * projection;
                    wvp = Convert(worldViewProjection);
                    ray = Mathematics.Ray3.FromScreen(mouse.X, mouse.Y, vp, wvp);

                    camera.Position = control.CameraPosition;
                    camera.View = Convert(view);

                    lightEntity.Transform.Translation = new Axiverse.Vector3(Functions.Sin(current), -1f, Functions.Cos(current)) * 10;
                    compositor.Draw(scene, dt);

                    device.Canvas.Begin();
                    {
                        while (frametime.Count > 20)
                        {
                            frametime.Dequeue();
                        }

                        device.Canvas.DrawString(1 / frametime.Average() + " fps ", 10, 60);

                        //var relative = RelativeFrame.FromBody(target.Body);
                        //device.Canvas.DrawString("Oriented Velocity: " + relative.LinearVelocity.ToString(2), 10, 130);
                        //device.Canvas.DrawString("Local Angular:" + relative.AngularVelocity.ToString(2), 10, 150);
                        //device.Canvas.DrawString("Position:" + target.Body.LinearPosition.ToString(2), 10, 170);
                        device.Canvas.DrawString("Mouse:" + mouse.ToString() + " " + ray.ToString(), 10, 200);

                        chrome.Update(dt);
                        chrome.Draw(device.Canvas);

                        device.Canvas.End();
                    }

                    device.Present();
                    previous = current;
                });
            }
        }
    }
}
