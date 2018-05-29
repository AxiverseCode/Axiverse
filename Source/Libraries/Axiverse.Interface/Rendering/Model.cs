using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Injection;

namespace Axiverse.Interface.Rendering
{
    /// <summary>
    /// Model
    /// </summary>
    public class Model
    {
        /// <summary>
        /// Gets the list of meshes in the model.
        /// </summary>
        public List<Mesh> Meshes { get; } = new List<Mesh>();

        /// <summary>
        /// Gets a list of the materials in the model.
        /// </summary>
        public List<Material> Materials { get; } = new List<Material>();
        public List<Model> Children { get; } = new List<Model>();
        public Model Parent;

        public readonly BindingDictionary Bindings = new BindingDictionary();
    }
}
