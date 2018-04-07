using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Rendering
{
    public class Bone
    {
        public int Identifier { get; set; }

        public int Parent { get; set; }
        public int SubmeshId { get; set; }
        public string Tag { get; set; }



        public enum Flags
        {
            SphericalBillboard,
            CylindricalBillboardX,
            CylindricalBillboardY,
            CylindricalBillboardZ,

            KinematicBone,
            HelmetScaled,
        }

        Track<Vector3> Translation;
        Track<Quaternion> Rotation;
        Track<Vector3> Scaling;
        Vector3 Pivot;
    }

    // special bones lookup.
}
