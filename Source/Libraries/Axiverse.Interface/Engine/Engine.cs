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

        public Form Form { get; private set; }

        public Scene Scene { get; set; }


        public Escapement SimulationEscapement { get; }
        public Universe Simulation { get; set; }

        public Window Window
        {
            get => Compositor.Window;
            set => Compositor.Window = value;
        }

        public Router Router { get; set; }


        public GraphicsDevice Device { get; private set; }
        public GraphicsDevice2D Device2D { get; set; }
        public Compositor Compositor { get; private set; }
        public Process Process { get; set; }
        public Presenter Presenter { get; set; }
        public CameraComponent Camera { get; set; }

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
            Injector.Bind<Universe>(Scene);
        }

        /// <summary>
        /// Initializes the engine.
        /// </summary>
        public void Initialize()
        {
            // Create a window
            Form = new Form()
            {
                ClientSize = new System.Drawing.Size(1600, 1200),
                Text = "Axiverse | Hello Graphics",
            };
            Form.Resize += (e, sender) =>
            {
                resize = true;
            };
            Form.Deactivate += (e, sender) =>
            {
                Router.Enabled = false;
            };
            Form.Activated += (e, sender) =>
            {
                Router.Enabled = true;
            };

            // Init the rendering device
            Device = GraphicsDevice.Create();
            var presenterDescription = new PresenterDescription()
            {
                Width = Form.ClientSize.Width,
                Height = Form.ClientSize.Height,
                WindowHandle = Form.Handle
            };

            Presenter = new Presenter(Device, presenterDescription);
            Presenter.Initialize();
            Device2D = GraphicsDevice2D.Create(Device, Presenter);

            Compositor = new Compositor(Device, Presenter);
            Compositor.Device2D = Device2D;

            // Bind resources
            Injector.Bind(Device);
        }


        bool resize = false;
        /// <summary>
        /// Run loop for the engine.
        /// </summary>
        public void Run()
        {
            Form.Show();

            var previousTick = Environment.TickCount;
            using (var loop = new RenderLoop(Form))
            {
                while (loop.NextFrame())
                {
                    var currentTick = Environment.TickCount;
                    var deltaTick = currentTick - previousTick;
                    previousTick = currentTick;

                    // Router.Poll();
                    // Step and run processors.
                    FloatingPoint.ThrowOnSevere = true;

                    Process?.OnFrame();

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
                        Presenter.Resize(Form.ClientSize.Width, Form.ClientSize.Height);
                        Presenter.TryResize();
                        Device2D.InitializePresentable(Device);
                    }

                    Compositor.Process(Scene, Camera);
                }
            }
        }
    }
}
