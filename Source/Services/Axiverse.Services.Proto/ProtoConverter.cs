using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ProtoVector3 = Axiverse.Services.Proto.Vector3;
using ProtoQuaternion = Axiverse.Services.Proto.Quaternion;

namespace Axiverse
{
    public static class ProtoConverter
    {
        public static Vector3 Convert(ProtoVector3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        public static ProtoVector3 Convert(Vector3 value)
        {
            return new ProtoVector3
            {
                X = value.X,
                Y = value.Y,
                Z = value.Z
            };
        }

        public static Quaternion Convert(ProtoQuaternion value)
        {
            return new Quaternion(value.X, value.Y, value.Z, value.W);
        }

        public static ProtoQuaternion Convert(Quaternion value)
        {
            return new ProtoQuaternion
            {
                X = value.X,
                Y = value.Y,
                Z = value.Z,
                W = value.W,
            };
        }
    }
}
