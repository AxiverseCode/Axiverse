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

        public Scene Scene { get; set; }


        public Escapement SimulationEscapement { get; }
        public Universe Simulation { get; set; }

        public Window Window { get; set; }

        public Router Router { get; set; }


        public GraphicsDevice Device { get; private set; }
        public GraphicsDevice2D Device2D { get; set; }
        public Compositor Compositor { get; private set; }

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

            SimulationEscapement = new Escapement();
            SimulationEscapement.Period = 1000 / 20;
            Simulation = new Universe();

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
            Device = GraphicsDevice.Create();
            var presenterDescription = new PresenterDescription()
            {
                Width = form.ClientSize.Width,
                Height = form.ClientSize.Height,
                WindowHandle = form.Handle
            };
            var presenter = new Presenter(Device, presenterDescription);
            presenter.Initialize();
            Device2D = GraphicsDevice2D.Create(Device, presenter);
            form.Resize += (e, sender) =>
            {
                resize = true;
            };

            Compositor = new Compositor(Device, presenter);
            Compositor.Device2D = Device2D;

            // Bind resources
            Injector.Bind(Device);

            var previousTick = Environment.TickCount;
            using (var loop = new RenderLoop(form))
            {
                while (loop.NextFrame())
                {
                    var currentTick = Environment.TickCount;
                    var deltaTick = currentTick - previousTick;
                    previousTick = currentTick;

                    // Router.Poll();
                    // Step and run processors.
                    FloatingPoint.ThrowOnSevere = true;

                    // Advance simulation.
                    if (SimulationEscapement.Advance(deltaTick))
                    {
                        Console.WriteLine("Simulation Advance");
                        Simulation.Step(SimulationEscapement.Period / 1000.0f);
                    }

                    //Console.WriteLine(delta);
                    Console.WriteLine("Scene Advance");
                    Scene.Step(deltaTick / 1000.0f);


                    if (resize)
                    {
                        Device2D.DisposeFrames();
                        presenter.Resize(form.ClientSize.Width, form.ClientSize.Height);
                        presenter.TryResize();
                        Device2D.InitializePresentable(Device);
                    }

                    Compositor.Process(Scene, null);
                }
            }
        }

        public void Process()
        {
            // Continuation.
            // Really hard to understand processes. 
            // Bad architecture leads to a lot of work but not a lot of progress.
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
