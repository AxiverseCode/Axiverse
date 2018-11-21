using Axiverse.Injection;
using System.Collections.Generic;

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

        /// <summary>
        /// List of child models which need to be iteratively rendered.
        /// </summary>
        public List<Model> Children { get; } = new List<Model>();

        /// <summary>
        /// The parent of this model.
        /// </summary>
        public Model Parent;

        /// <summary>
        /// Other bindings.
        /// </summary>
        public readonly BindingDictionary Bindings = new BindingDictionary();
    }
}
