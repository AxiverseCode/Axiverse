using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Engine;
using Axiverse.Interface2.Entites;
using Axiverse.Interface2.Interface;
using Axiverse.Interface2.Models;
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

        public ProgramEngine(CoreEngine engine) : base(engine)
        {
            pbr = new PhysicallyBasedRenderer(engine.Device);
            sbr = new SkyboxRenderer(engine.Device);

            float ratio = (float)Engine.Form.ClientRectangle.Width / Engine.Form.ClientRectangle.Height;
            projection = Matrix4.PerspectiveFovLH(3.14F / 3.0F, ratio, 1, 1000);
            view = Matrix4.LookAtLH(new Vector3(0, 0, -30),
                new Vector3(0, 1, 0), Vector3.UnitY);
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
            menu.Items[1].Children.AddRange("Cut", "Copy", "Paste");
            menu.Items[4].Children.AddRange("About");
            Chrome.Controls.Add(menu);

            var entityTree = new Interface.Custom.EntityComponentTree(Scene)
            {
                Position = new Vector2(10, 500),
                Size = new Vector2(160, 300),
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
            var shipModel = Model.FromMesh(Engine.Device, Mathematics.Geometry.WavefrontObj.Load("../../Model.obj"));
            shipModel.Materials.Add(material);
            var boxModel = Model.FromMesh(Engine.Device, Mathematics.Geometry.Mesh.CreateCube().Invert().CalculateNormals());

            Scene.Entities.Add(new Entity("Camera")
                .Add(new Camera
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
                    Scaling = new Axiverse.Vector3(30),
                })
                .Add(new Renderable()
                {
                    Model = shipModel,
                    Renderer = pbr,
                }));

            Scene.Entities.Add(new Entity("Point Light",
                new Transform
                {
                    Scaling = new Axiverse.Vector3(4, 5, 6),
                })
                .Add(new Light()
                {
                    Color = new Vector4(0.6f, 0.9f, 0.8f, 1),
                    Intensity = 1,
                }));
        }

        static void Main(string[] args)
        {
            using (var engine = new Engine.CoreEngine())
            {
                engine.Process = new ProgramEngine(engine);
                engine.Run();
            }
        }
    }
}
