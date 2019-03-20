using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    /// <summary>
    /// A convex 3d hexahedron. Used for Frustums as well.
    /// </summary>
    public struct Hexahedron3
    {
        /// <summary>Gets or sets the A plane.</summary>
        public Plane3 A;

        /// <summary>Gets or sets the B plane.</summary>
        public Plane3 B;

        /// <summary>Gets or sets the C plane.</summary>
        public Plane3 C;

        /// <summary>Gets or sets the D plane.</summary>
        public Plane3 D;

        /// <summary>Gets or sets the E plane.</summary>
        public Plane3 E;

        /// <summary>Gets or sets the F plane.</summary>
        public Plane3 F;

        // https://github.com/sharpdx/SharpDX/blob/master/Source/SharpDX.Mathematics/BoundingFrustum.cs
    }
}
