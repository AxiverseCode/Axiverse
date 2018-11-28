using System;
using Axiverse.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Mathematics.Numerics.Interpolation;

namespace Axiverse.Interface.Rendering.Animation
{
    /// <summary>
    /// A track for a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITrack<T>
    {
        /// <summary>
        /// Gets the length of the track.
        /// </summary>
        float Length { get; }

        /// <summary>
        /// Gets the value of the track at a frame.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        T this[float frame] { get; }
    }

    /// <summary>
    /// A keyframe for a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IKeyframe<T>
    {
        /// <summary>
        /// The position of the keyframe.
        /// </summary>
        float Position { get; }

        /// <summary>
        /// Gets the absolute value of at the specified position.
        /// </summary>
        T Value { get; }
    }

    /// <summary>
    /// Transformation track which combines translation, rotation, and scaling.
    /// </summary>
    public class TransformTrack : ITrack<Matrix4>
    {
        public float Length { get; set; }

        public ITrack<Vector3> Translation { get; set; }
        public ITrack<Quaternion> Rotation { get; set; }
        public ITrack<Vector3> Scaling { get; set; }

        public Matrix4 this[float frame]
        {
            get
            {
                return Matrix4.Transformation(Scaling[frame], Rotation[frame], Translation[frame]);
            }
        }
    }

    public class Track<TType, TKey> : ITrack<TType>
        where TKey : IKeyframe<TType>
    {
        public float Length { get; set; }

        public SortedList<float, TKey> List { get; } = new SortedList<float, TKey>();

        public TType this[float frame]
        {
            get
            {
                var index = List.Keys.BinarySearch(frame);
                if (index > 0)
                {
                    return List.Values[index].Value;
                }

                index = ~index;
                if (index == List.Count)
                {
                    return List.Values[index - 1].Value;
                }
                if (index == 0)
                {
                    return List.Values[0].Value;
                }

                var lowerPosition = List.Keys[index - 1];
                var upperPosition = List.Keys[index];
                var scale = frame - lowerPosition / (upperPosition - lowerPosition);

                return Interpolate(index - 1, index, scale);
            }
        }

        protected virtual TType Interpolate(int lower, int upper, float scale)
        {
            return Interpolate(List.Values[lower], List.Values[upper], scale);
        }

        protected virtual TType Interpolate(TKey lower, TKey upper, float scale)
        {
            return lower.Value;
        }
    }

    public class Keyframe<T> : IKeyframe<T>
    {
        public float Position { get; set; }
        public T Value { get; set; }
    }

    public class HermiteKey : Keyframe<Vector3>
    {
        public Vector3 Out { get; set; }
        public Vector3 In { get; set; }
    }


    public class VectorLerpTrack : Track<Vector3, Keyframe<Vector3>>
    {
        protected override Vector3 Interpolate(Keyframe<Vector3> lower, Keyframe<Vector3> upper, float scale)
        {
            return Vector3.Lerp(lower.Value, upper.Value, scale);
        }
    }

    public class HermiteTrack : Track<Vector3, HermiteKey>
    {
        protected override Vector3 Interpolate(HermiteKey lower, HermiteKey upper, float scale)
        {
            return Hermite.Interpolate(lower.Value, lower.In, upper.Value, upper.Out, scale);
        }
    }

    public class LerpTrack : Track<Quaternion, Keyframe<Quaternion>>
    {
        protected override Quaternion Interpolate(Keyframe<Quaternion> lower, Keyframe<Quaternion> upper, float scale)
        {
            return Quaternion.Lerp(lower.Value, upper.Value, scale);
        }
    }

    public class SlerpTrack : Track<Quaternion, Keyframe<Quaternion>>
    {
        protected override Quaternion Interpolate(Keyframe<Quaternion> lower, Keyframe<Quaternion> upper, float scale)
        {
            return Quaternion.Slerp(lower.Value, upper.Value, scale);
        }
    }
}