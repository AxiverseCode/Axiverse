using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Injection;

namespace Axiverse.Interface.Rendering
{
    public class Model
    {
        public readonly List<Mesh> Meshes = new List<Mesh>();
        public readonly List<Material> Materials = new List<Material>();
        public readonly List<Model> Children = new List<Model>();
        public Model Parent;

        public readonly BindingDictionary Bindings = new BindingDictionary();
    }
}
