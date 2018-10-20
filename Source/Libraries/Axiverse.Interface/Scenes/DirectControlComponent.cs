using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Scenes
{
    public class DirectControlComponent : Component
    {
        public Vector3 Steering { get; set; }

        public Vector3 Translational { get; set; }
    }
}
