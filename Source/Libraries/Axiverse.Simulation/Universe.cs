using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public class Universe
    {
        public Universe()
        {
            for (int i = 0; i < 5; i++)
            {
                var entity = new Entity();
                entities.Add(entity.Identifier, entity);
            }
        }

        public void Post(Guid target)
        {

        }

        public void Step(float dt)
        {
            //Console.WriteLine("===== Stepping =====");

            foreach (var entity in entities.Values)
            {
                entity.Model?.Process(entity);
                //Console.WriteLine(entity);
            }
        }

        private readonly Dictionary<Guid, Entity> entities = new Dictionary<Guid, Entity>();
    }
}
