using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Interface.Graphics;

namespace Axiverse.Interface
{
    public class Scene
    {
        public List<Entity> Entities { get; } = new List<Entity>();
        public Camera Camera { get; set; }
    }
}
