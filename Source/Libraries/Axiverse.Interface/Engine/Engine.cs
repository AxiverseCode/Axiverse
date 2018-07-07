using Axiverse.Injection;
using Axiverse.Interface.Graphics;
using Axiverse.Interface.Graphics.Generic;
using Axiverse.Interface.Graphics.Shaders;
using Axiverse.Interface.Input;
using Axiverse.Interface.Rendering;
using Axiverse.Interface.Rendering.Compositing;
using Axiverse.Interface.Scenes;
using Axiverse.Interface.Windows;
using Axiverse.Resources;
using Axiverse.Simulation;
using SharpDX;
using SharpDX.Windows;
using System;

namespace Axiverse.Interface.Engine
{
    /// <summary>
    /// An interface engine.
    /// </summary>
    public class Engine
    {
        public Injector Injector { get; private set; }
        public Cache Cache { get; private set; }

        RenderForm form;

        public Compositor Compositor { get; set; }

        public Scene Scene { get; set; }

        public Window Window { get; set; }

        public Router Router { get; set; }

        /// <summary>
        /// Constructs an engine.
        /// </summary>
        /// <param name="injector"></param>
        /// <param name="cache"></param>
        [Inject]
        public Engine(Injector injector, Cache cache)
        {
            Injector = injector;
            Cache = cache;
            Scene = new Scene();
            Router = new Router();
            Injector.Bind<Universe>(Scene);

            Scene.Add(new TransformProcessor());
            Scene.Add(new CameraProcessor());
        }

        /// <summary>
        /// Initializes the engine.
        /// </summary>
        public void Initialize()
        {
            // Create a window
            form = new RenderForm()
            {
                ClientSize = new System.Drawing.Size(1600, 1200),
                Text = "Axiverse | Hello Graphics",
            };
        }

        /// <summary>
        /// Run loop for the engine.
        /// </summary>
        public void Run()
        {
            form.Show();

            // Init the rendering device
            var device = GraphicsDevice.Create();
            var presenterDescription = new PresenterDescription()
            {
                Width = form.ClientSize.Width,
                Height = form.ClientSize.Height,
                WindowHandle = form.Handle
            };
            var presenter = new Presenter(device, presenterDescription);
            presenter.Initialize();
            var device2d = GraphicsDevice2D.Create(device, presenter);


            Window = new Window();
            Window.Bounds = new Rectangle(0, 0, presenter.Description.Width, presenter.Description.Height);
            Window.Bind(form);
            Control.DefaultFont = new Font("Open Sans", 16, Windows.FontWeight.Normal);

            var control = new Dialog();
            control.Bounds = new Rectangle(10, 10, 200, 400);
            control.BackgroundColor = new Color(0, 1, 0);
            Window.Children.Add(control);

            control.Children.Add(new Control()
            {
                Bounds = new Rectangle(10, 100, 50, 50),
                BackgroundColor = Colors.Red,
            });
            control.Children.Add(new Button()
            {
                Location = new Vector2(10, 50),
            });


            var compositor = new Compositor(device, presenter);
            compositor.Device2D = device2d;
            compositor.Window = Window;

            // Bind resources
            Injector.Bind(device);

            // Shaders
            var frame = 0.0f;

            var texture = new Texture(device);
            texture.Load(@".\Resources\Textures\Placeholder Grid.jpg");

            var skymap = new Texture(device);
            skymap.Load(@".\Resources\Textures\NASA Starmap 8k.jpg");

            // Lets create some resources
            LoadCube(device);
            LoadSphere(device);
            LoadShip(device);

            var sky = new Entity();
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
            var cameraEntity = new Entity();
            var cameraComponent = cameraEntity.Components.Add(new CameraComponent
            {
                Projection = Matrix4.PerspectiveFovRH(Functions.DegreesToRadians(60.0f),
                    1.0f * form.ClientSize.Width / form.ClientSize.Height,
                    0.5f,
                    2000.0f),
                Mode = CameraMode.Forward
            });
            var cameraTransform = cameraEntity.Components.Add(new TransformComponent
            {
                Translation = new Vector3(0, 0, 10)
            });
            cameraTransform.Children.Add(sky);
            Scene.Add(cameraEntity);


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
                Mesh = new Mesh { Draw = Cache.Load<MeshDraw>("memory:cube").Value }
            });
            entity2.Components.Get<RenderableComponent>().Mesh.Bindings.Add(texture);

            var entity3 = new Entity();
            Scene.Add(entity3);
            entity3.Components.Add(new TransformComponent
            {
                Scaling = new Vector3(10, 10, 10)
            });
            entity3.Components.Add(new RenderableComponent
            {
                Mesh = new Mesh { Draw = Cache.Load<MeshDraw>("memory:ship").Value }
            });
            entity3.Components.Get<RenderableComponent>().Mesh.Bindings.Add(texture);

            var maximumVelocity = new Vector3(10, 10, 10);
            var maximumAngle = new Vector3(3, 3, 3);

            var prev = Environment.TickCount;
            // Into the loop we go!
            using (var loop = new RenderLoop(form))
            {
                while (loop.NextFrame())
                {
                    var next = Environment.TickCount;
                    var delta = next - prev;
                    var dt = delta / 1000.0f;
                    frame += delta / 10.0f;
                    prev = next;


                    entity1.Components.Get<TransformComponent>().Translation = new Vector3(
                        3 * Functions.Sin(Functions.DegreesToRadians(frame / 3)),
                        2 * Functions.Sin(Functions.DegreesToRadians(frame / 6)),
                        4 * Functions.Cos(Functions.DegreesToRadians(frame / 8)));
                    entity2.Components.Get<TransformComponent>().Rotation = Quaternion.FromEuler(frame / 100f, frame / 747, frame / 400);

                    //cameraTransform.Translation = new Vector3(
                    //    10 * Functions.Sin(Functions.DegreesToRadians(frame / 10)),
                    //    4 * Functions.Sin(Functions.DegreesToRadians(frame / 30)),
                    //    10 * Functions.Cos(Functions.DegreesToRadians(frame / 10)));

                    Router.Poll();
                    var state = Router.State;
                    var velocity = cameraTransform.Rotation.Transform(new Vector3(state.X, -state.Z, state.Y)) * maximumVelocity;
                    //var angular = new Vector3(state.RotationX, state.RotationY, state.RotationZ) * maximumAngle;
                    var angular = new Vector3(state.RotationY, -state.RotationZ, state.RotationX) * maximumAngle;
                    //Console.WriteLine(velocity);
                    //Console.WriteLine(angular);

                    cameraTransform.Translation += velocity;
                    cameraTransform.Rotation *= Quaternion.FromEuler(angular);

                    Scene.Step(delta / 1000.0f);


                    compositor.Process(Scene, cameraComponent);
                }
            }
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

            Cache.Add("memory:cube", meshDraw);
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

            Cache.Add("memory:sphere", meshDraw);
        }

        public void LoadShip(GraphicsDevice device)
        {
            var spaceMesh = Assets.Models.WavefrontObjMesh.Load(device, @".\Resources\Models\ship.obj");
            var spaceMeshDraw = new MeshDraw
            {
                VertexBuffers = new[] { spaceMesh },
                Count = spaceMesh.Count,
            };

            Cache.Add("memory:ship", spaceMeshDraw);
        }
    }
}
