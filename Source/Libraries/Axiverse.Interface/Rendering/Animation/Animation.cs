using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Rendering.Animation
{
    /// <summary>
    /// An animation for a specific skeleton.
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// Gets the skeleton for the animation.
        /// </summary>
        public Skeleton Skeleton { get; set; }

        /// <summary>
        /// Gets or sets the name of the animation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the length of the animation.
        /// </summary>
        public float Length { get; set; }

        /// <summary>
        /// Gets or sets the transformation tracks for the animation.
        /// </summary>
        public List<ITrack<Matrix4>> Transforms { get; } = new List<ITrack<Matrix4>>();

        /// <summary>
        /// Gets the transforms for a particular frame.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public Matrix4[] this[float frame]
        {
            get
            {
                var result = new Matrix4[Skeleton.Bones.Count];
                Compute(result, frame);
                return result;
            }
        }

        /// <summary>
        /// Computes the transforms for a particular frame.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="frame"></param>
        public void Compute(Matrix4[] buffer, float frame)
        {
            Requires.That(Transforms.Count == Skeleton.Bones.Count, "Transform track count must match bone count.");

            // Compute initial transforms.
            for (int i = 0; i < Transforms.Count; i++)
            {
                buffer[i] = Transforms[i][frame];
            }

            // Cascade dependencies.
            for (int i = 0; i < Skeleton.Bones.Count; i++)
            {
                if (Skeleton.Bones[i].Parent != null)
                {
                    buffer[i] = buffer[Skeleton.Bones[i].Parent.Index] * buffer[i];
                }
            }
        }
    }
}
