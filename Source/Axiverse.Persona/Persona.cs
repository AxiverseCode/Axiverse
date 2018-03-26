using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Persona
{
    public class Persona
    {
        public Guid Identifier { get; set; }

        public String Name { get; set; }

        public int Variance { get; set; }

        public String Email { get; set; }

        public String Salt { get; set; }

        public String Password { get; set; }

        
    }
}
