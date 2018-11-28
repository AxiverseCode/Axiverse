using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Rendering.Animation
{
    /// <summary>
    /// The transformations for a moment in an animation.
    /// </summary>
    public class Moment
    {
        private float frame;
        private Animation animation;

        /// <summary>
        /// Gets the skeleton this moment is bound to.
        /// </summary>
        public Skeleton Skeleton { get; }

        /// <summary>
        /// Gets or sets the current frame of the moment.
        /// </summary>
        public float Frame
        {
            get => frame;
            set
            {
                frame = value;
                Animation.Compute(Buffer, frame);
            }
        }

        /// <summary>
        /// Gets or sets the animation of the moment. The animation must be bound to the same
        /// skeleton as the moment.
        /// </summary>
        public Animation Animation
        {
            get => animation;
            set
            {
                Requires.That(value.Skeleton == Skeleton);
                animation = value;
            }
        }

        /// <summary>
        /// Gets the transform buffer for all the bones.
        /// </summary>
        public Matrix4[] Buffer { get; }

        /// <summary>
        /// Constructs a moment.
        /// </summary>
        /// <param name="skeleton"></param>
        public Moment(Skeleton skeleton)
        {
            Skeleton = skeleton;
            Buffer = new Matrix4[Skeleton.Bones.Count];
        }
    }
}
