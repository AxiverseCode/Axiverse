using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Rendering.Animation
{
    /// <summary>
    /// An bone on a skeleton.
    /// </summary>
    public class Bone
    {
        /// <summary>
        /// Gets or sets the name of the bone.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the index of the bone.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the parent of the bone.
        /// </summary>
        public Bone Parent { get; set; }
    }
}
