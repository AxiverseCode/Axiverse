using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Prototypes
{
    public class EntityPrototype
    {
        public float Structure { get; set; }

        public float Shields { get; set; }

        public float Energy { get; set; }

        public string Name { get; set; }

        public Entity Create()
        {
            var value = new Entity();

            value.Structure.Maximum = Structure;
            value.Shields.Maximum = Shields;
            value.Energy.Maximum = Energy;

            value.Prototype = this;

            return value;
        }
    }
}
