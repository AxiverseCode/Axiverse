using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Spatial
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISpatial3
    {
        Sphere3 BoundingSphere { get; }

        /// <summary>
        /// 
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// 
        /// </summary>
        float Radius { get; }

        /// <summary>
        /// 
        /// </summary>
        event EventHandler PositionChanged;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler RadiusChanged;
    }
}
