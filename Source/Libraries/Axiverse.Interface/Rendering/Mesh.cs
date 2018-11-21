using Axiverse.Injection;

namespace Axiverse.Interface.Rendering
{
    /// <summary>
    /// Part of an model grouped by material.
    /// </summary>
    public class Mesh
    {
        // Fixed buffers, fixed material slots for PBR?

        /// <summary>
        /// Gets or sets the mesh geometry drawing information.
        /// </summary>
        public MeshDraw Draw;

        /// <summary>
        /// Gets or sets the material index to be used.
        /// </summary>
        public int MaterialIndex;

        /// <summary>
        /// Extranious parameters.
        /// </summary>
        public readonly BindingDictionary Bindings = new BindingDictionary();
    }
}
