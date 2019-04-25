using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Entities;

namespace Axiverse.Interface2.Engine
{
    public class CompositingContext
    {
        public float Time;
        public float DeltaTime;

        public Camera Camera { get; set; }
        public List<Light> Lights { get; set; }

        public Light[] ClosestLights(Vector3 position)
        {
            return (Lights.Count == 0) ? new Light[0] : new Light[] { Lights[0] };
        }
    }
}
