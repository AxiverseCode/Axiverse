using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface.Scenes;
using Axiverse.Interface.Rendering.Compositing;

namespace Axiverse.Interface.Engine
{
    public class SceneSystem
    {
        public Scene Scene { get; set; }

        public Compositor Compositor { get; set; }
    }
}
