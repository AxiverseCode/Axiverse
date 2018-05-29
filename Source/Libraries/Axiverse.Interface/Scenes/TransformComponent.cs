using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Simulation;
namespace Axiverse.Interface.Scenes
{
    public class TransformComponent : Component
    {
        /// <summary>
        /// Gets or sets the transform relative to the global basis.
        /// </summary>
        public Matrix4 GlobalTransform { get; set; }

        /// <summary>
        /// Gets or sets the local transform relative to the parent.
        /// </summary>
        public Matrix4 LocalTransform { get; set; }

        /// <summary>
        /// Gets or sets the translation.
        /// </summary>
        public Vector3 Translation { get; set; }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// Gets or sets the scaling.
        /// </summary>
        public Vector3 Scaling { get; set; }

        /// <summary>
        /// Clones the component.
        /// </summary>
        /// <returns></returns>
        public override Component Clone()
        {
            throw new NotImplementedException();
        }
    }
}
