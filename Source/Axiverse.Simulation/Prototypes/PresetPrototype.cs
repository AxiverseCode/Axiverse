using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation.Prototypes
{
    public class PresetPrototype
    {
        public Prototypes Prototypes { get; set; }

        public string Name { get; set; }

        public string Carrier { get; set; }

        public List<string> Equiptment { get; set; }

        public Entity Create()
        {
            var value = Prototypes.Entities[Carrier].Create();

            foreach (var equiptment in Equiptment)
            {
                if (Prototypes.Equiptment.TryGetValue(equiptment, out var e))
                {
                    value.Equiptment.Add(e.Create());
                }
                else
                {
                    Console.WriteLine($"Error: Unable to load equiptment {equiptment}");
                }
            }

            return value;
        }
    }
}
