using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Models
{
    public class Material
    {
        public Texture2D Albedo { get; set; }
        public Texture2D Normal { get; set; }
        public Texture2D Roughness { get; set; }
        public Texture2D Specular { get; set; }
    }
}
