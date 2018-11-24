using Axiverse.Injection;
using Axiverse.Interface.Graphics;
using Axiverse.Interface.Graphics.Generic;
using Axiverse.Interface.Input;
using Axiverse.Interface.Rendering;
using Axiverse.Interface.Rendering.Compositing;
using Axiverse.Interface.Scenes;
using Axiverse.Interface.Windows;
using Axiverse.Physics;
using Axiverse.Resources;
using Axiverse.Simulation;
using Axiverse.Simulation.Behaviors;
using SharpDX.Windows;
using System;
using System.Collections.Generic;

namespace Axiverse.Interface.Engine
{
    /// <summary>
    /// An interface engine.
    /// </summary>
    public class Engine
    {
        /// <summary>
        /// Gets the injector.
        /// </summary>
        public Injector Injector { get; private set; }

        /// <summary>
        /// Gets the object cache.
        /// </summary>
        public Cache Cache { get; private set; }

        Form form;

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
            Router.Listeners.Add(sixAxisListner);
            Router.Listeners.Add(twoAxisListener);
            Injector.Bind<Universe>(Scene);

            Scene.Add(new TransformPhysicsProcessor());
            Scene.Add(new TransformProcessor());
            Scene.Add(new CameraProcessor());
            Scene.Add(new BehaviorProcessor());
            Scene.Add(new DirectControlProcessor());
        }

        SixAxisListener sixAxisListner = new SixAxisListener();
        TwoAxisListener twoAxisListener = new TwoAxisListener();

        /// <summary>
        /// Initializes the engine.
        /// </summary>
        public void Initialize()
        {
            // Create a window
            form = new Form()
            {
                ClientSize = new System.Drawing.Size(1600, 1200),
                Text = "Axiverse | Hello Graphics",
            };
        }


        bool resize = false;
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
            form.ResizeEnd += (e, sender) =>
            {
                resize = true;
            };

            Window = new Window();
            Window.Bounds = new Rectangle(0, 0, presenter.Description.Width, presenter.Description.Height);
            Window.Bind(form);
            Control.DefaultFont = new Font("Open Sans", 16, Windows.FontWeight.Normal);

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


            var compositor = new Compositor(device, presenter);
            compositor.Device2D = device2d;
            compositor.Window = Window;

            // Bind resources
            Injector.Bind(device);

            // Shaders
            var frame = 0.0f;

            var texture = Texture.Load(device, @".\Resources\Textures\Placeholder Grid.jpg");
            var uvGrid = Texture.Load(device, @".\Resources\Textures\UV Grid.png");
            var skymap = Texture.Load(device, @".\Resources\Textures\NASA Starmap 4k.jpg");

            // Lets create some resources
            LoadCube(device);
            LoadSphere(device);
            LoadModel(device, "ship");
            // Blender is Z top to bottom, Y front to back, X right to left.
            // X right to left, Z
            LoadModel(device, "drone", new Matrix3(m11: 1, m23: -1, m32: 1));

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
                    1.0f * form.ClientSize.Width / form.ClientSize.Height,
                    0.5f,
                    2000.0f),
                Mode = CameraMode.Forward
            });
            var cameraTransform = cameraEntity.Components.Add(new TransformComponent
            {
                Translation = Vector3.BackwardRH * 10,
                Inheritance = TransformInheritance.Rotation | TransformInheritance.Translation
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

            var maximumVelocity = Vector3.One;
            var maximumAngle = Vector3.One / 6.28f;

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

                    var mappedLinear = new Vector3(twoAxisListener.Position.X, 0, -twoAxisListener.Position.Y);
                    var mappedAngular = new Vector3(-twoAxisListener.Position2.Y, -twoAxisListener.Position2.X, 0);
                    // body.ApplyCentralLocalImpulse(new Vector3(twoAxisListener.Position.X, 0, -twoAxisListener.Position.Y));
                    // body.ApplyLocalTorqueImpulse(new Vector3(-twoAxisListener.Position2.Y, -twoAxisListener.Position2.X, 0) * dt * 100);
                    controller.Translational = mappedLinear;
                    controller.Steering = mappedAngular;

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
                    var velocity = cameraTransform.Rotation.Transform(new Vector3(sixAxisListner.Translation.X, -sixAxisListner.Translation.Z, sixAxisListner.Translation.Y)) * maximumVelocity;
                    //var angular = new Vector3(state.RotationX, state.RotationY, state.RotationZ) * maximumAngle;
                    var angular = new Vector3(sixAxisListner.Rotation.Y, -sixAxisListner.Rotation.Z, sixAxisListner.Rotation.X) * maximumAngle;
                    sixAxisListner.Acknowledge();
                    //Console.WriteLine(velocity);
                    //Console.WriteLine(angular);

                    cameraTransform.Translation += velocity;
                    cameraTransform.Rotation *= Quaternion.FromEuler(angular);

                    // Step and run processors.
                    FloatingPoint.ThrowOnSevere = true;

                    //Console.WriteLine(delta);
                    Scene.Step(delta / 1000.0f);

                    if (resize)
                    {
                        device2d.DisposeFrames();
                        presenter.Resize(form.ClientSize.Width, form.ClientSize.Height);
                        presenter.TryResize();
                        device2d.InitializePresentable(device);
                    }

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

        public void LoadModel(GraphicsDevice device, string name, Matrix3? transform = null)
        {
            var spaceMesh = Assets.Models.WavefrontObjMesh.Load(device, $@".\Resources\Models\{name}.obj", transform);
            var spaceMeshDraw = new MeshDraw
            {
                VertexBuffers = new[] { spaceMesh },
                Count = spaceMesh.Count,
            };

            Cache.Add("memory:" + name, spaceMeshDraw);
        }
    }
}
