using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Scenes
{
    /// <summary>
    /// Represents a specific view.
    /// </summary>
    public class SceneView
    {
        public string Name { get; private set; }

        public CameraComponent Camera { get; set; }
    }
}
