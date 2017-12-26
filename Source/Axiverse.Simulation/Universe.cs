using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public class Universe
    {
        public double Time { get; private set; }
        public long Step { get; set; }

        public List<Entity> Entitites = new List<Entity>();

        public void OnStep(float delta)
        {
            Time += delta;
            Step++;
            //Console.WriteLine($"Universe: Simulation time {Time} step {Step}.");
 
            foreach (var entity in Entitites)
            {
                entity.Step(delta);
            }

            //Console.ReadKey();
        }

        public void Run()
        {

            while (true)
            {
                
                float delta = 0.1f;
                OnStep(delta);
            }
        }

    }
}
