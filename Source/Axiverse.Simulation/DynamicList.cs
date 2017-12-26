using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public class DynamicList<T> : List<T>, IDynamic where T : IDynamic
    {
        public void Step(float delta)
        {
            foreach (T value in this)
            {
                value.Step(delta);
            }
        }
    }
}
