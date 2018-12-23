using Axiverse.Interface.Assets.Models;
using Axiverse.Interface.Engine;
using Axiverse.Interface.Graphics;
using Axiverse.Interface.Graphics.Generic;
using Axiverse.Interface.Rendering;
using Axiverse.Interface.Scenes;
using Axiverse.Interface.Windows;
using Axiverse.Physics;
using Axiverse.Resources;
using Axiverse.Simulation;
using Axiverse.Simulation.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Calibration
{
    public class CalibrartionProcess : Process
    {
        public Engine Engine { get; private set; }
        public GraphicsDevice Device => Engine.Device;
        public Cache Cache => Engine.Cache;
        public Scene Scene => Engine.Scene;
        public Window Window
        {
            get => Engine.Window;
            set => Engine.Window = value;
        }

        public Entity Ship { get; set; }
        public TrackballControl Trackball { get; set; }

        public CalibrartionProcess(Engine engine)
        {
            Engine = engine;
        }

        public Entity Camera { get; set; }

        public override void OnInitialize()
        {
            Engine.Scene.Add(new TransformPhysicsProcessor());
            Engine.Scene.Add(new TransformProcessor());
            Engine.Scene.Add(new CameraProcessor());
            Engine.Scene.Add(new BehaviorProcessor());
            Engine.Scene.Add(new DirectControlProcessor());

            Window = new Window();
            Window.MouseDown += Window_MouseDown;
            Window.MouseMove += Window_MouseMove;
            Window.MouseUp += Window_MouseUp;
            Window.MouseWheel += Window_MouseWheel;
            Window.Bounds = new Rectangle(0, 0, Engine.Presenter.Description.Width, Engine.Presenter.Description.Height);
            Window.Bind(Engine.Form);
            Control.DefaultFont = new Font("Open Sans", 16, FontWeight.Normal);

            var control = new Dialog();
            control.Bounds = new Rectangle(10, 10, 200, 400);
            control.BackgroundColor = Colors.Blue;
            control.ForegroundColor = Colors.Yellow;
            Window.Children.Add(control);

            control.Children.Add(new Control()
            {
                Bounds = new Rectangle(10, 100, 50, 50),
                BackgroundColor = Colors.Red,
                ForegroundColor = Colors.Yellow,
            });
            control.Children.Add(new Button()
            {
                Location = new Vector2(10, 50),
            });


            var texture = Texture.Load(Device, @".\Resources\Textures\Placeholder Grid.jpg");
            var uvGrid = Texture.Load(Device, @".\Resources\Textures\UV Grid.png");
            var skymap = Texture.Load(Device, @".\Resources\Textures\NASA Starmap 4k.jpg");

            // Lets create some resources
            LoadCube(Device);
            LoadSphere(Device);
            LoadModel(Device, "ship");
            // Blender is Z top to bottom, Y front to back, X right to left.
            // X right to left, Z
            LoadModel(Device, "drone", new Matrix3(m11: 1, m23: -1, m32: 1));

            var sky = new Entity("sky");
            Scene.Add(sky);
            sky.Components.Add(new TransformComponent()
            {
                Inheritance = TransformInheritance.Translation
            });
            sky.Components.Add(new RenderableComponent()
            {
                Mesh = new Mesh { Draw = Cache.Load<MeshDraw>("memory:sphere").Value }
            });
            sky.Components.Get<RenderableComponent>().Mesh.Bindings.Add(skymap);

            // Create camera entity.
            var cameraEntity = new Entity("camera");
            var cameraComponent = cameraEntity.Components.Add(new CameraComponent
            {
                Projection = Matrix4.PerspectiveFovRH(Functions.DegreesToRadians(60.0f),
                    1.0f * Engine.Form.ClientSize.Width / Engine.Form.ClientSize.Height,
                    0.5f,
                    2000.0f),
                Mode = CameraMode.Targeted,
            });
            var cameraTransform = cameraEntity.Components.Add(new TransformComponent
            {
                Translation = Vector3.BackwardRH * 10,
                Inheritance = TransformInheritance.Rotation | TransformInheritance.Translation
            });
            cameraTransform.Children.Add(sky);
            Scene.Add(cameraEntity);
            Camera = cameraEntity;
            Engine.Camera = cameraComponent;

            Trackball = new TrackballControl
            {
                Enabled = true,
                PanEnabled = false,
                RotateEnabled = true,
                ZoomEnabled = true,
                CameraPosition = Vector3.BackwardRH * 10,
                Up = Vector3.Up,
                Screen = new Rectangle(0, 0, Engine.Form.ClientSize.Width, Engine.Form.ClientSize.Height)
            };


            var entity1 = new Entity();
            Scene.Add(entity1);
            entity1.Components.Add(new TransformComponent());
            entity1.Components.Add(new RenderableComponent()
            {
                Mesh = new Mesh { Draw = Cache.Load<MeshDraw>("memory:cube").Value }
            });
            entity1.Components.Get<RenderableComponent>().Mesh.Bindings.Add(texture);

            var entity2 = new Entity();
            Scene.Add(entity2);
            entity2.Components.Add(new TransformComponent());
            entity2.Components.Add(new RenderableComponent
            {
                Mesh = new Mesh
                {
                    Draw = Cache.Load<MeshDraw>("memory:cube").Value
                }
            });
            entity2.Components.Get<RenderableComponent>().Mesh.Bindings.Add(texture);

            var shipEntity = new Entity("ship");
            Scene.Add(shipEntity);
            var shipTransform = shipEntity.Components.Add(new TransformComponent
            {
                Scaling = new Vector3(10, 10, 10)
            });
            shipTransform.Children.Add(cameraEntity);
            shipEntity.Components.Add(new RenderableComponent
            {
                Mesh = new Mesh { Draw = Cache.Load<MeshDraw>("memory:ship").Value }
            });
            shipEntity.Components.Get<RenderableComponent>().Mesh.Bindings.Add(texture);
            Ship = shipEntity;
            var body = new Body();
            //body.LinearDampening *= 0.99f;
            //body.AngularDampening *= 0.99f;
            body.LinearPosition = Vector3.ForwardLH * 10;
            shipEntity.Components.Add(new PhysicsComponent(body));
            var controller = shipEntity.Components.Add(new DirectControlComponent());

            //body.ApplyTorqueImpulse(Vector3.UnitY * 0.1f);


            List<Entity> flock = new List<Entity>();
            for (int i = 0; i < 10; i++)
            {
                var bloid = new Entity();
                bloid.Components.Add(new TransformComponent
                {
                    Scaling = new Vector3(0.2f, 0.2f, 0.2f)
                });
                bloid.Components.Add(new BehaviorComponent());
                bloid.Components.Add(new RenderableComponent()
                {
                    Mesh = new Mesh { Draw = Cache.Load<MeshDraw>("memory:drone").Value }
                });
                bloid.Components.Get<RenderableComponent>().Mesh.Bindings.Add(uvGrid);
                bloid.Components.Add(new PhysicsComponent(new Body
                {
                    LinearPosition = Functions.Random.NextVector3(-10, 10),
                    LinearVelocity = Functions.Random.NextVector3(-10, 10),
                    AngularPosition = Functions.Random.NextQuaternion()
                }));

                Scene.Add(bloid);
                flock.Add(bloid);
            }
        }

        private void Window_MouseWheel(object sender, MouseMoveEventArgs e)
        {
            Trackball.OnMouseWheel(e.DeltaZ);
        }

        private void Window_MouseUp(object sender, MouseEventArgs e)
        {
            Trackball.OnMouseUp();
        }

        private void Window_MouseMove(object sender, MouseMoveEventArgs e)
        {
            Trackball.OnMouseMove(new Vector2(e.X, e.Y));
            System.Diagnostics.Debug.WriteLine(new Vector2(e.X, e.Y));
        }

        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            Trackball.OnMouseDown(e.Button, new Vector2(e.X, e.Y));
        }

        public override void OnFrame()
        {
            base.OnFrame();
            Trackball.Update();
            Camera.Components.Get<CameraComponent>().Target = Trackball.Target;
            Camera.Components.Get<CameraComponent>().up = Trackball.Up;
            Camera.Components.Get<TransformComponent>().Translation = Trackball.CameraPosition;
        }

        public void LoadCube(GraphicsDevice device)
        {
            var cube = Primitives<PositionColorTexture>.Cube();
            var indices = cube.Item1;
            var vertices = cube.Item2;
            var indexBuffer = GraphicsBuffer.Create(device, indices, false);

            var vertexBuffer = GraphicsBuffer.Create(device, vertices, false);
            var indexBinding = new IndexBufferBinding
            {
                Buffer = indexBuffer,
                Count = indices.Length,
                Type = IndexBufferType.Integer32,
                //Offset = 0,
            };
            var vertexBinding = new VertexBufferBinding
            {
                Buffer = vertexBuffer,
                Count = vertices.Length,
                Stride = PositionColorTexture.Layout.Stride,
                //Offset = 0,
            };
            var meshDraw = new MeshDraw
            {
                IndexBuffer = indexBinding,
                VertexBuffers = new[] { vertexBinding },
                Count = indices.Length,
            };

            Engine.Cache.Add("memory:cube", meshDraw);
        }

        public void LoadSphere(GraphicsDevice device)
        {
            var cube = Primitives<PositionColorTexture>.Sphere(1500, 20, 20);
            var indices = cube.Item1;
            var vertices = cube.Item2;
            var indexBuffer = GraphicsBuffer.Create(device, indices, false);

            var vertexBuffer = GraphicsBuffer.Create(device, vertices, false);
            var indexBinding = new IndexBufferBinding
            {
                Buffer = indexBuffer,
                Count = indices.Length,
                Type = IndexBufferType.Integer32,
                //Offset = 0,
            };
            var vertexBinding = new VertexBufferBinding
            {
                Buffer = vertexBuffer,
                Count = vertices.Length,
                Stride = PositionColorTexture.Layout.Stride,
                //Offset = 0,
            };
            var meshDraw = new MeshDraw
            {
                IndexBuffer = indexBinding,
                VertexBuffers = new[] { vertexBinding },
                Count = indices.Length,
            };

            Engine.Cache.Add("memory:sphere", meshDraw);
        }

        public void LoadModel(GraphicsDevice device, string name, Matrix3? transform = null)
        {
            var spaceMesh = WavefrontObjMesh.Load(device, $@".\Resources\Models\{name}.obj", transform);
            var spaceMeshDraw = new MeshDraw
            {
                VertexBuffers = new[] { spaceMesh },
                Count = spaceMesh.Count,
            };

            Engine.Cache.Add("memory:" + name, spaceMeshDraw);
        }
    }
}
