using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Rendering.Animation
{
    /// <summary>
    /// A skeleton with bones and animations.
    /// </summary>
    public class Skeleton
    {
        /// <summary>
        /// Gets the list of bones.
        /// </summary>
        public List<Bone> Bones { get; } = new List<Bone>();

        /// <summary>
        /// Gets the dictionary of animations.
        /// </summary>
        public Dictionary<string, Animation> Animations { get; } = new Dictionary<string, Animation>();

        /// <summary>
        /// Validates the ordering of the bones.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            for (int i = 0; i < Bones.Count; i++)
            {
                var bone = Bones[i];
                Requires.That(bone.Index == i);
                Requires.That(bone.Parent.Index < bone.Index);
                Requires.That(Bones[bone.Parent.Index] == bone.Parent);
            }

            return true;
        }
    }
}
