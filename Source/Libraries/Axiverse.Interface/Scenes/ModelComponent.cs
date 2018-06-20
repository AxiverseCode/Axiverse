using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Simulation;
using Axiverse.Interface.Rendering;
using Axiverse.Mathematics;
namespace Axiverse.Interface.Scenes
{
    public class ModelComponent : Component
    {
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public Model Model { get; set; }

        /// <summary>
        /// Gets or sets the bounding box in global space.
        /// </summary>
        public Bounds3 BoundingBox { get; set; }

        /// <summary>
        /// Gets or sets the bounding sphere in global space.
        /// </summary>
        public Sphere3 BoundingSphere { get; set; }

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
