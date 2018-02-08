using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Windows;
using Axiverse.Interface.Graphics;
using Axiverse.Interface.Windows;

using Axiverse.Simulation;
using Axiverse.Simulation.Prototypes;

namespace Axiverse.Interface
{
    public class Engine2
    {
        public RenderForm Target { get; private set; }
        public Renderer Renderer { get; private set; }

        public SceneGraph Scene { get; } = new SceneGraph();
        public Window Window { get; set; } = new Window();
        public Camera Camera;
        public TrackballController Trackball;

        public Entity entity;
        public Simulation.Entity ship;
        public Universe Universe;
        public Prototypes Prototypes;

        public void Initialize()
        {
            Target = new RenderForm()
            {
                Width = 1200,
                Height = 1000,
                BackColor = System.Drawing.Color.Black,
                // Icon = new System.Drawing.Icon("../../Assets/cake1.ico"),
                Text = "Axiverse | Interface",
            };
            Window.Bind(Target);
            
            Renderer = new Renderer();
            Renderer.Initialize(Target/*, Interface*/);
            Renderer.Pipelines.Add(new GeometryPipeline(Renderer, Scene));
            Renderer.Pipelines.Add(new VoxelPipeline(Renderer, Scene));
            Renderer.Pipelines.Add(new WindowsPipeline(Renderer, Window));
            Target.Resize += (s, e) => Renderer.RenderTarget.Resize(Target);

            var mesh = new ObjMesh(Renderer);
            mesh.Load(@"Models/ship.obj");
            //mesh.Populate(Primitives<PositionColorTexture>.Cube());

            var texture = new Texture(Renderer);
            texture.Load(@"Textures/Placeholder Grid.jpg");

            entity = new Entity();
            entity.AddComponent(new ModelComponent()
            {
                Mesh = mesh,
                Texture = texture,
            });
            entity.Transform.Transformation = Matrix.RotationYawPitchRoll(1, 1, 1);
            Scene.Entities.Add(entity);

            entity = new Entity();
            entity.AddComponent(new ModelComponent()
            {
                Mesh = mesh,
                Texture = texture,
            });
            entity.Transform.Transformation = Matrix.RotationYawPitchRoll(1, 2, 1) * Matrix.Translation(2, 1, 1);
            Scene.Entities.Add(entity);

            Scene.Camera = Camera = new Camera(Renderer);
            Trackball = new TrackballController(Camera, Window);
            Trackball.MaximumDistance = 50;


            //Prototypes = new Prototypes(@"..\..\Data");
            //Universe = new Universe();
            //Universe.Entitites.Add(ship = Prototypes.Presets["Auto Shuttle"].Create());
        }

        public void Start()
        {
            Target.Show();

            //var engine = new Audio.AudioEngine();
            //var sfx = new Audio.Audio(engine, System.IO.File.Open(@"Assets\Sounds\0022_spaceship user - lightspeed space wind.mp3", System.IO.FileMode.Open));

            using (var loop = new RenderLoop(Target))
            {
                while (loop.NextFrame())
                {
                    //Universe.OnStep(0.01f);
                    //var q = ship.AngularPosition;
                    //var x = ship.LinearPosition;
                    //entity.Transform.Transformation = Matrix.RotationQuaternion(new SharpDX.Quaternion(q.X, q.Y, q.Z, q.W)) * Matrix.Translation(x.X, x.Y, x.Z);
                    //Camera.Target = new SharpDX.Vector3(x.X, x.Y, x.Z);
                    //Window.Children[0].Text = $"{x.ToString(0)} -> {q.ToString(2)}";

                    Trackball.Update();
                    Renderer.Execute();
                }
            }

            //sfx.Dispose();
            //engine.Dispose();
        }
    }
}
