using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Engine;
using Axiverse.Interface2.Interface;
using SharpDX;

namespace Axiverse.Interface2
{
    class ProgramEngine : EngineProcess
    {
        protected internal override void OnEnter(Engine.Engine engine)
        {
            Chrome.Attach(engine.Form);

            var menu = new Interface.Menu()
            {
                Position = new Vector2(),
                Backcolor = new Color(0.2f, 0.2f, 0.2f, 0.8f),
                Forecolor = Color.White,
                Size = new Vector2(9000, 40)
            };
            menu.Items.Add(new MenuItem("File"));
            menu.Items[0].Children.Add(new MenuItem("New"));
            menu.Items[0].Children.Add(new MenuItem("Open"));
            menu.Items[0].Children.Add(new MenuItem("Save"));
            menu.Items[0].Children.Add(new MenuItem("Exit"));
            menu.Items.Add(new Interface.MenuItem("Edit"));
            menu.Items[1].Children.Add(new MenuItem("Cut"));
            menu.Items[1].Children.Add(new MenuItem("Copy"));
            menu.Items[1].Children.Add(new MenuItem("Paste"));
            menu.Items.Add(new MenuItem("View"));
            menu.Items.Add(new MenuItem("Window"));
            menu.Items.Add(new MenuItem("Help"));
            menu.Items[4].Children.Add(new MenuItem("About"));
            Chrome.Controls.Add(menu);

        }

        static void Main(string[] args)
        {
            using (var engine = new Engine.Engine())
            {
                engine.Process = new ProgramEngine();
                engine.Run();
            }
        }
    }
}
