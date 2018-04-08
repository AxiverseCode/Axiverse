using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Injection;

namespace Axiverse.Interface.Rendering
{
    public class Mesh
    {
        public MeshDraw Draw;
        public int MaterialIndex;
        public readonly BindingDictionary Bindings = new BindingDictionary();
        // Parameters

    }
}
