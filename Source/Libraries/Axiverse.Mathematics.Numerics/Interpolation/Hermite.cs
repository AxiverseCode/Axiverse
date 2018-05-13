using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Numerics.Interpolation
{
    class Hermite
    {

        public static float Interpolate(float inPosition, float inTangent, float outPosition, float outTangent, float t)
        {
            float a = -inPosition / 2.0f + (3.0f * inTangent) / 2.0f - (3.0f * outPosition) / 2.0f + outTangent / 2.0f;
            float b = inPosition - (5.0f * inTangent) / 2.0f + 2.0f * outPosition - outTangent / 2.0f;
            float c = -inPosition / 2.0f + outPosition / 2.0f;
            float d = inTangent;

            return a * t * t * t + b * t * t + c * t + d;
        }

        public static Vector2 Interpolate(Vector2 inPosition, Vector2 inTangent, Vector2 outPosition, Vector2 outTangent, float t)
        {
            return new Vector2(
                Interpolate(inPosition.X, inTangent.X, outPosition.X, outTangent.X, t),
                Interpolate(inPosition.Y, inTangent.Y, outPosition.Y, outTangent.Y, t));
        }

        public static Vector3 Interpolate(Vector3 inPosition, Vector3 inTangent, Vector3 outPosition, Vector3 outTangent, float t)
        {
            return new Vector3(
                Interpolate(inPosition.X, inTangent.X, outPosition.X, outTangent.X, t),
                Interpolate(inPosition.Y, inTangent.Y, outPosition.Y, outTangent.Y, t),
                Interpolate(inPosition.Z, inTangent.Z, outPosition.Z, outTangent.Z, t));
        }

        public static Vector4 Interpolate(Vector4 inPosition, Vector4 inTangent, Vector4 outPosition, Vector4 outTangent, float t)
        {
            return new Vector4(
                Interpolate(inPosition.X, inTangent.X, outPosition.X, outTangent.X, t),
                Interpolate(inPosition.Y, inTangent.Y, outPosition.Y, outTangent.Y, t),
                Interpolate(inPosition.Z, inTangent.Z, outPosition.Z, outTangent.Z, t),
                Interpolate(inPosition.W, inTangent.W, outPosition.W, outTangent.W, t));
        }
    }
}
