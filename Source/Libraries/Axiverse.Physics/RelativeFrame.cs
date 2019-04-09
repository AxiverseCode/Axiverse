using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Physics
{
    public struct RelativeFrame
    {
        public Vector3 LinearPosition { get; set; }
        public Quaternion AngularPosition { get; set; }

        public Vector3 LinearVelocity { get; set; }
        public Vector3 AngularVelocity { get; set; }

        public static RelativeFrame FromBody(Body local)
        {
            return new RelativeFrame()
            {
                LinearVelocity = local.AngularPosition.InverseTransform(local.LinearVelocity),
                AngularVelocity = local.AngularPosition.InverseTransform(local.AngularVelocity),
            };
        }

        public static RelativeFrame FromBody(Body local, Body target)
        {
            var relativeGlobalLinearPosition = target.LinearPosition - local.LinearPosition;
            var relativeGlobalLinearVelocity = target.LinearVelocity - local.LinearVelocity;
            
            return new RelativeFrame()
            {
                LinearPosition = local.AngularPosition.InverseTransform(relativeGlobalLinearPosition),
                LinearVelocity = local.AngularPosition.InverseTransform(relativeGlobalLinearVelocity),
            };
        }

        public static RelativeFrame FromPoint(Body local, Vector3 position, Vector3 velocity = new Vector3())
        {
            var relativeGlobalLinearPosition = position - local.LinearPosition;
            var relativeGlobalLinearVelocity = velocity - local.LinearVelocity;

            return new RelativeFrame()
            {
                LinearPosition = local.AngularPosition.InverseTransform(relativeGlobalLinearPosition),
                LinearVelocity = local.AngularPosition.InverseTransform(relativeGlobalLinearVelocity),
            };
        }

        public static Vector3 ToGlobalPoint(Body local, Vector3 point)
        {
            return local.AngularPosition.Transform(point) + local.LinearPosition;
        }
    }
}
