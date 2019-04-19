using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Entites;

namespace Axiverse.Interface2.Engine
{
    public class CompositingContext
    {
        public Camera Camera { get; set; }
        public List<Light> Lights { get; set; }
    }
}
