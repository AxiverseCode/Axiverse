using Axiverse.Injection;
using Axiverse.Interface.Graphics;
using Axiverse.Interface.Graphics.Generic;
using Axiverse.Interface.Graphics.Shaders;
using Axiverse.Interface.Rendering;
using Axiverse.Interface.Rendering.Compositing;
using Axiverse.Interface.Scenes;
using Axiverse.Interface.Windows;
using Axiverse.Resources;
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
        }

        /// <summary>
        /// Initializes the engine.
        /// </summary>
        public void Initialize()
        {
            // Create a window
            form = new RenderForm()
            {
                Width = 1200,
                Height = 800,
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
            Control.DefaultFont = new Windows.Font("Open Sans", 16, Windows.FontWeight.Normal);

            var control = new Dialog();
            control.Bounds = new Rectangle(10, 10, 400, 400);
            control.BackgroundColor = new Windows.Color(.4f);
            Window.Children.Add(control);

            control.Children.Add(new Windows.Control()
            {
                Bounds = new Rectangle(10, 50, 50, 50),
                BackgroundColor = Windows.Colors.Yellow,
            });

            {
                var z = new Windows.Button()
                {
                    Location = new Vector2(100, 50),
                };

                control.Children.Add(z);
            }


            var compositor = new Compositor(device, presenter);
            compositor.Device2D = device2d;
            compositor.Window = Window;

            // Bind resources
            Injector.Bind(device);

            // Shaders
            var frame = 0.0f;

            var texture = new Texture(device);
            texture.Load(@".\Resources\Textures\Placeholder Grid.jpg");


            // Lets create some resources
            LoadCube(device);
            LoadShip(device);
           
            // Create camera entity.
            var cameraEntity = new Entity();
            var cameraComponent = new CameraComponent
            {
                Projection = Matrix4.PerspectiveFovRH(Functions.DegreesToRadians(60.0f),
                    1.0f * form.ClientSize.Width / form.ClientSize.Height,
                    2.0f,
                    2000.0f)
            };
            cameraEntity.Components.Add(cameraComponent);
            Scene.Add(cameraEntity);

            var entity1 = new Entity();
            Scene.Add(entity1);
            entity1.Components.Add(new TransformComponent());
            entity1.Components.Add(new RenderableComponent()
            {
                Mesh = new Mesh { Draw = Cache.Load<MeshDraw>("memory:cube").Value}
            });
            entity1.Components.Get<RenderableComponent>().Mesh.Bindings.Add(texture);

            var entity2 = new Entity();
            Scene.Add(entity2);
            entity2.Components.Add(new TransformComponent());
            entity2.Components.Add(new RenderableComponent()
            {
                Mesh = new Mesh { Draw = Cache.Load<MeshDraw>("memory:cube").Value }
            });
            entity2.Components.Get<RenderableComponent>().Mesh.Bindings.Add(texture);

            var entity3 = new Entity();
            Scene.Add(entity3);
            entity3.Components.Add(new TransformComponent());
            entity3.Components.Add(new RenderableComponent()
            {
                Mesh = new Mesh { Draw = Cache.Load<MeshDraw>("memory:ship").Value }
            });
            entity3.Components.Get<RenderableComponent>().Mesh.Bindings.Add(texture);

            var prev = Environment.TickCount;
            // Into the loop we go!
            using (var loop = new RenderLoop(form))
            {
                while (loop.NextFrame())
                {
                    var next = Environment.TickCount;
                    frame += (next - prev) / 10.0f;
                    prev = next;

                    entity1.Components.Get<TransformComponent>().GlobalTransform = Matrix4.Identity;
                    entity2.Components.Get<TransformComponent>().GlobalTransform =
                        Matrix4.FromQuaternion(Quaternion.FromEuler(frame / 100f, frame / 747, frame / 400));
                    entity3.Components.Get<TransformComponent>().GlobalTransform = Matrix4.Scale(10, 10, 10);
                    
                    cameraComponent.View = Matrix4.LookAtRH(
                        new Vector3(
                            10 * Functions.Sin(Functions.DegreesToRadians(frame / 10)),
                            4 * Functions.Sin(Functions.DegreesToRadians(frame / 30)),
                            10 * Functions.Cos(Functions.DegreesToRadians(frame / 10))),
                        new Vector3(0, 0, 0), new Vector3(0, 1, 0));
                    
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
