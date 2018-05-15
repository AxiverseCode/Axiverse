using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Simulation;

namespace Axiverse.Interface.Scenes
{
    public class Scene
    {
        public Universe Universe { get; set; }

        public Entity Root { get; set; }
    }
}
