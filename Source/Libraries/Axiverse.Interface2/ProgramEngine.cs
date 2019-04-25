using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Engine;
using Axiverse.Interface2.Entities;
using Axiverse.Interface2.Interface;
using Axiverse.Interface2.Models;
using Axiverse.Interface2.Simulation;
using SharpDX;

namespace Axiverse.Interface2
{
    using Color = Axiverse.Mathematics.Drawing.Color;

    class ProgramEngine : EngineProcess
    {
        PhysicallyBasedRenderer pbr;
        SkyboxRenderer sbr;
        Matrix4 projection;
        Matrix4 view;
        Camera camera;
        TrackballControl control;

        public Simulator Simulator { get; private set; } = new Simulator();

        public ProgramEngine(CoreEngine engine) : base(engine)
        {
            pbr = new PhysicallyBasedRenderer(engine.Device);
            sbr = new SkyboxRenderer(engine.Device);
            control = new TrackballControl
            {
                CameraPosition = new Vector3(0, 0, -50),
                ZoomEnabled = true,
                RotateEnabled = true,
                Screen = new Rectangle(0, 0, engine.Form.ClientSize.Width, engine.Form.ClientSize.Height),
                Enabled = true,
                Up = new Vector3(0, 1, 0)
            };

            control.Bind(engine.Form);
            engine.Form.Resize += HandleResize;
            HandleResize(null, null);

            Simulator.Scene = Scene;

            // Inputs
            Simulator.Stages.Add(new SensorStage(Scene));

            // Process
            Simulator.Stages.Add(new LogicStage(Scene));

            // Outputs
            Simulator.Stages.Add(new PhysicsStage(Scene));
        }

        private void HandleResize(object sender, EventArgs e)
        {
            float ratio = (float)Engine.Form.ClientRectangle.Width / Engine.Form.ClientRectangle.Height;
            projection = Matrix4.PerspectiveFovLH(3.14F / 3.0F, ratio, 1, 1000);
            view = Matrix4.LookAtLH(new Vector3(0, 0, -90),
                new Vector3(0, 1, 0), Vector3.UnitY);
            if (camera != null)
            {
                camera.Projection = projection;
                camera.View = view;
            }
        }

        protected override void OnUpdate(Clock clock)
        {
            Simulator.Update(clock);
            control.Update();
            view = Matrix4.LookAtLH(control.CameraPosition, control.Target, control.Up);
            camera.View = view;
            camera.Position = control.CameraPosition;
        }

        protected internal override void OnEnter(Engine.CoreEngine engine)
        {
            Chrome.Attach(engine.Form);

            var menu = new Interface.Menu()
            {
                Position = new Vector2(),
                Size = new Vector2(9000, 40)
            };
            menu.Items.AddRange("File", "Edit", "View", "Window", "Help");
            menu.Items[0].Children.AddRange("New", "Open", "Save", "Exit");
            menu.Items[0].Children[3].Clicked += (s, e) => engine.Form.Close();
            menu.Items[1].Children.AddRange("Cut", "Copy", "Paste");
            menu.Items[4].Children.AddRange("About");
            Chrome.Controls.Add(menu);

            var entityTree = new Interface.Custom.EntityComponentTree(Scene)
            {
                Position = new Vector2(10, 100),
                Size = new Vector2(200, 400),
            };
            Chrome.Controls.Add(entityTree);



            var skyBox = Texture2D.FromFile(Engine.Device,
                "../../skybox/right.jpg",
                "../../skybox/left.jpg",
                "../../skybox/top.jpg",
                "../../skybox/bottom.jpg",
                "../../skybox/front.jpg",
                "../../skybox/back.jpg");
            pbr.skybox = skyBox;
            sbr.skybox = skyBox;

            var material = new Material()
            {
                Albedo = Texture2D.FromFile(Engine.Device, "../../pbr/albedo.jpg"),
                Normal = Texture2D.FromFile(Engine.Device, "../../pbr/normal.jpg"),
                Roughness = Texture2D.FromFile(Engine.Device, "../../pbr/roughness.jpg"),
                Height = Texture2D.FromFile(Engine.Device, "../../pbr/height.jpg"),
                Specular = Texture2D.FromFile(Engine.Device, "../../pbr/metallic.jpg"),
                Alpha = Texture2D.FromFile(Engine.Device, "../../pbr/alpha.jpg"),
                Occlusion = Texture2D.FromFile(Engine.Device, "../../pbr/ambientocclusion.jpg"),
            };
            material.Roughness = material.Height;
            //material.Specular = material.Normal;

            var aMaterial = new Material()
            {
                Albedo = Texture2D.FromColor(Engine.Device, Color.Gray),
                Normal = Texture2D.FromColor(Engine.Device, new Color(0.5f, 1f, 0.5f)),
                Roughness = Texture2D.FromColor(Engine.Device, new Color(0.8f)),
                Specular = Texture2D.FromColor(Engine.Device, new Color(0.2f)),
                Occlusion = Texture2D.FromColor(Engine.Device, Color.White),
            };

            var shipModel = Model.FromMesh(Engine.Device, Mathematics.Geometry.WavefrontObj.Load("../../Model.obj"));
            shipModel.Materials.Add(material);
            var missileModel = Model.FromMesh(Engine.Device, Mathematics.Geometry.WavefrontObj.Load("../../Missile.obj"));
            missileModel.Materials.Add(material);
            var boxModel = Model.FromMesh(Engine.Device, Mathematics.Geometry.Mesh.CreateCube().Invert().CalculateNormals());

            var rock1 = Model.FromMesh(Engine.Device, Mathematics.Geometry.WavefrontObj.Load("../../Rock1.obj"));
            rock1.Materials.Add(aMaterial);
            var rock2 = Model.FromMesh(Engine.Device, Mathematics.Geometry.WavefrontObj.Load("../../Rock2.obj"));
            rock2.Materials.Add(aMaterial);

            Scene.Entities.Add(new Entity("Camera")
                .Add(camera = new Camera
                {
                    View = view,
                    Projection = projection,
                })
                .Add(new Renderable
                {
                    Model = boxModel,
                    Renderer = sbr,
                }));

            Scene.Entities.Add(new Entity("Ship 1",
                new Transform
                {
                    Scaling = new Axiverse.Vector3(5),
                })
                .Add(new Renderable()
                {
                    Model = rock2,
                    Renderer = pbr,
                })
                .Add(new Physical()));

            Scene.Entities.Add(new Entity("Point Light",
                new Transform
                {
                    Translation = new Axiverse.Vector3(4, 50, 6),
                })
                .Add(new Light()
                {
                    Color = new Vector4(1f, 1),
                    Intensity = 1,
                }));

            for (int i = 0; i < 100; i++)
            {
                Physical p;
                Scene.Entities.Add(new Entity("Missile " + (i + 1),
                    new Transform
                    {
                        Scaling = new Axiverse.Vector3(0.2f, 1, 0.2f),
                    })
                    .Add(new Renderable()
                    {
                        Model = missileModel,
                        Renderer = pbr,
                    })
                    .Add(new Agent())
                    .Add(p = new Physical()));
                p.Body.AngularPosition = Functions.Random.NextQuaternion();
            }
        }

        static void Main(string[] args)
        {
            // Network.NetworkTimeProtocol.Test();

            using (var engine = new Engine.CoreEngine())
            {
                engine.Process = new ProgramEngine(engine);
                engine.Run();
            }
        }
    }
}
