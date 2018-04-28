using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse
{
    /// <summary>
    /// Represents a screen space rectangle.
    /// </summary>
    public struct Rectangle
    {
        /// <summary>
        /// Gets the x value of the rectangle.
        /// </summary>
        public float X;

        /// <summary>
        /// Gets the y value of the rectangle.
        /// </summary>
        public float Y;

        /// <summary>
        /// Gets the width of the rectangle.
        /// </summary>
        public float W;

        /// <summary>
        /// Gets the height of the rectangle.
        /// </summary>
        public float H;

        /// <summary>
        /// Gets the width of the rectangle.
        /// </summary>
        public float Width { get => W; set => W = value; }

        /// <summary>
        /// Gets the height of the rectangle.
        /// </summary>
        public float Height { get => H; set => H = value; }

        /// <summary>
        /// Gets the left value of the rectangle.
        /// </summary>
        public float Left { get => X; set => X = value; }

        /// <summary>
        /// Gets the top of the rectangle.
        /// </summary>
        public float Top { get => Y; set => Y = value; }

        /// <summary>
        /// Gets the right value of the rectangle.
        /// </summary>
        public float Right => X + W;

        /// <summary>
        /// Gets the bottom value of the rectangle.
        /// </summary>
        public float Bottom => Y + H;

        /// <summary>
        /// Gets the location of the rectangle.
        /// </summary>
        public Vector2 Location { get => new Vector2(Left, Top); set { Left = value.X; Top = value.Y; } }

        /// <summary>
        /// Gets the size of the rectangle.
        /// </summary>
        public Vector2 Size { get => new Vector2(Width, Height); set { Width = value.X; Height = value.Y; } }

        /// <summary>
        /// Gets the top left point of the rectangle.
        /// </summary>
        public Vector2 TopLeft => new Vector2(Left, Top);

        /// <summary>
        /// Gets the top right point of the rectangle.
        /// </summary>
        public Vector2 TopRight => new Vector2(Right, Top);

        /// <summary>
        /// Gets the bottom left point of the rectangle.
        /// </summary>
        public Vector2 BottomLeft => new Vector2(Left, Bottom);

        /// <summary>
        /// Gets the bottom left point of the rectangle.
        /// </summary>
        public Vector2 BottomRight => new Vector2(Right, Bottom);

        /// <summary>
        /// Constructs a rectangle.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle(float left, float top, float width, float height)
        {
            X = left;
            Y = top;
            W = width;
            H = height;
        }

        /// <summary>
        /// Determines whether the given point is inside the rectangle.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Vector2 point)
        {
            return (point.X >= Left && point.X < Right) && (point.Y >= Top && point.Y < Bottom);
        }

        /// <summary>
        /// Gets the string representation of this rectangle.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"X = {X}, Y = {Y}, Width = {W}, Height = {H}";
        }
    }
}
