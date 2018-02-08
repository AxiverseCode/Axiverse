using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Scene
{
    /// <summary>
    /// Represents on a 
    /// </summary>
    public class SceneComponent : Component
    {
        public EntityCollection Children { get; } = new EntityCollection();
    }
}
