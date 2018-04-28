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
        public float X;
        public float Y;
        public float W;
        public float H;

        public float Width { get => W; set => W = value; }
        public float Height { get => H; set => H = value; }
        public float Left { get => X; set => X = value; }
        public float Top { get => Y; set => Y = value; }
        public float Right => X + W;
        public float Bottom => Y + H;

        public Vector2 Location { get => new Vector2(Left, Top); set { Left = value.X; Top = value.Y; } }
        public Vector2 Size { get => new Vector2(Width, Height); set { Width = value.X; Height = value.Y; } }
        public Vector2 TopLeft => new Vector2(Left, Top);
        public Vector2 TopRight => new Vector2(Right, Top);
        public Vector2 BottomLeft => new Vector2(Left, Bottom);
        public Vector2 BottomRight => new Vector2(Right, Bottom);


        public Rectangle(float left, float top, float width, float height)
        {
            X = left;
            Y = top;
            W = width;
            H = height;
        }

        public bool Contains(Vector2 point)
        {
            return (point.X >= Left && point.X < Right) && (point.Y >= Top && point.Y < Bottom);
        }

        public override string ToString()
        {
            return $"X = {X}, Y = {Y}, Width = {W}, Height = {H}";
        }
    }
}
