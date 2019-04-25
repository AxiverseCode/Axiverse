using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Engine;
using Axiverse.Interface2.Models;

namespace Axiverse.Interface2.Entities
{
    public class Renderable : Component
    {
        public Renderer Renderer { get; set; }
        public Model Model { get; set; }
    }
}
