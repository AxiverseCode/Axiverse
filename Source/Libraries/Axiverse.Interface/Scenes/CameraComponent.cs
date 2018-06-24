using Axiverse.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Scenes
{
    public class CameraComponent : Component
    {
        public Matrix4 View { get; set; }
        public Matrix4 Projection { get; set; }
    }
}
