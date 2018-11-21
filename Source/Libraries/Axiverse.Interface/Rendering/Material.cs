using Axiverse.Injection;
using Axiverse.Interface.Graphics;

namespace Axiverse.Interface.Rendering
{
    /// <summary>
    /// Material attached to a mesh.
    /// </summary>
    public class Material
    {
        /// <summary>
        /// Various parameters including textures, constants, etc.
        /// </summary>
        public readonly BindingDictionary Bindings = new BindingDictionary();

        /// <summary>
        /// Gets or sets the albiedo texture.
        /// </summary>
        public Texture Albiedo { get; set; }

        /// <summary>
        /// Gets or sets the normal texture.
        /// </summary>
        public Texture Normal { get; set; }

        /// <summary>
        /// Gets or sets the roughtness texture.
        /// </summary>
        public Texture Roughness { get; set; }

        /// <summary>
        /// Gets or sets the ambient occlusion texture.
        /// </summary>
        public Texture AmbientOcclusion { get; set; }
    }
}
