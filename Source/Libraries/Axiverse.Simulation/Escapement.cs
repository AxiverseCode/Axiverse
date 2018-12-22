using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    public class Escapement
    {
        public int Period { get; set; }

        private int next;
        private int current;

        public bool Advance(int milliseconds)
        {
            current += milliseconds;

            if (current > next)
            {
                next += Period;
                return true;
            }

            return false;
        }
    }
}
