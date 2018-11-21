using System;
using System.Collections.Generic;

namespace Axiverse.Interface.Graphics.Generic
{
    public static class Primitives<T> where T : struct, IVertex
    {
        public static Tuple<uint[], T[]> Cube()
        {
            uint[] indices = new uint[]{
                0,  1,  2,  0,  2,  3,  // front
                4,  5,  6,  4,  6,  7,  // back
                8,  9,  10, 8,  10, 11, // top
                12, 13, 14, 12, 14, 15, // bottom
                16, 17, 18, 16, 18, 19, // right
                20, 21, 22, 20, 22, 23  // left
            };

            T[] vertices = new T[]{
                //front
                Vertex( new Vector3(-1, -1,  1), new Vector3(0, 0, 0), new Vector2(0, 0), new Vector4(1, 0, 0, 0) ),
                Vertex( new Vector3(-1,  1,  1), new Vector3(0, 0, 0), new Vector2(0, 1), new Vector4(1, 0, 0, 0) ),
                Vertex( new Vector3( 1,  1,  1), new Vector3(0, 0, 0), new Vector2(1, 1), new Vector4(1, 0, 0, 0) ),
                Vertex( new Vector3( 1, -1,  1), new Vector3(0, 0, 0), new Vector2(1, 0), new Vector4(1, 0, 0, 0) ),

                // back
                Vertex( new Vector3(-1, -1, -1), new Vector3(0, 0, 0), new Vector2(0, 0), new Vector4(0, 1, 0, 0) ),
                Vertex( new Vector3( 1, -1, -1), new Vector3(0, 0, 0), new Vector2(0, 1), new Vector4(0, 1, 0, 0) ),
                Vertex( new Vector3( 1,  1, -1), new Vector3(0, 0, 0), new Vector2(1, 1), new Vector4(0, 1, 0, 0) ),
                Vertex( new Vector3(-1,  1, -1), new Vector3(0, 0, 0), new Vector2(1, 0), new Vector4(0, 1, 0, 0) ),

                // top
                Vertex( new Vector3(-1,  1, -1), new Vector3(0, 0, 0), new Vector2(0, 0), new Vector4(0, 0, 1, 0) ),
                Vertex( new Vector3( 1,  1, -1), new Vector3(0, 0, 0), new Vector2(0, 1), new Vector4(0, 0, 1, 0) ),
                Vertex( new Vector3( 1,  1,  1), new Vector3(0, 0, 0), new Vector2(1, 1), new Vector4(0, 0, 1, 0) ),
                Vertex( new Vector3(-1,  1,  1), new Vector3(0, 0, 0), new Vector2(1, 0), new Vector4(0, 0, 1, 0) ),

                // bottom
                Vertex( new Vector3(-1, -1, -1), new Vector3(0, 0, 0), new Vector2(0, 0), new Vector4(0, 0, 0, 1) ),
                Vertex( new Vector3(-1, -1,  1), new Vector3(0, 0, 0), new Vector2(0, 1), new Vector4(0, 0, 0, 1) ),
                Vertex( new Vector3( 1, -1,  1), new Vector3(0, 0, 0), new Vector2(1, 1), new Vector4(0, 0, 0, 1) ),
                Vertex( new Vector3( 1, -1, -1), new Vector3(0, 0, 0), new Vector2(1, 0), new Vector4(0, 0, 0, 1) ),

                // right
                Vertex( new Vector3( 1, -1, -1), new Vector3(0, 0, 0), new Vector2(0, 0), new Vector4(1, 1, 0, 0) ),
                Vertex( new Vector3( 1, -1,  1), new Vector3(0, 0, 0), new Vector2(0, 1), new Vector4(1, 1, 0, 0) ),
                Vertex( new Vector3( 1,  1,  1), new Vector3(0, 0, 0), new Vector2(1, 1), new Vector4(1, 1, 0, 0) ),
                Vertex( new Vector3( 1,  1, -1), new Vector3(0, 0, 0), new Vector2(1, 0), new Vector4(1, 1, 0, 0) ),

                // let
                Vertex( new Vector3(-1, -1, -1), new Vector3(0, 0, 0), new Vector2(0, 0), new Vector4(1, 0, 1, 0) ),
                Vertex( new Vector3(-1,  1, -1), new Vector3(0, 0, 0), new Vector2(0, 1), new Vector4(1, 0, 1, 0) ),
                Vertex( new Vector3(-1,  1,  1), new Vector3(0, 0, 0), new Vector2(1, 1), new Vector4(1, 0, 1, 0) ),
                Vertex( new Vector3(-1, -1,  1), new Vector3(0, 0, 0), new Vector2(1, 0), new Vector4(1, 0, 1, 0) ),
            };

            return new Tuple<uint[], T[]>(indices, vertices);
        }

        public static Tuple<uint[], T[]> Sphere(float radius, int meridians, int parallels)
        {
            Requires.That(meridians >= 3);
            Requires.That(parallels >= 3);

            List<T> vertices = new List<T>();

            var up = Vector3.Up;

            vertices.Add(Vertex(Vector3.Up * radius, Vector3.Up, Vector2.Zero, Vector4.One));

            for (int j = 0; j < parallels - 1; j++)
            {
                var u = (float)(j + 1) / parallels;
                var polar = (float)Math.PI * u;
                var sinPolar = Functions.Sin(polar);
                var cosPolar = Functions.Cos(polar);

                for (int i = 0; i <= meridians; i++)
                {
                    var v = (float)i / meridians;
                    var azimuth = 2f * (float)Math.PI * v;
                    var sinAzimuth = Functions.Sin(azimuth);
                    var cosAzimuth = Functions.Cos(azimuth);
                    var point = new Vector3(
                        sinPolar * cosAzimuth,
                        cosPolar,
                        sinPolar * sinAzimuth);

                    vertices.Add(Vertex(point * radius, point, new Vector2(v, u), Vector4.One));
                }
            }

            vertices.Add(Vertex(Vector3.Down * radius, Vector3.Down, Vector2.One, Vector4.One));

            List<uint> indices = new List<uint>();

            for (uint i = 0; i < meridians; i++)
            {
                var a = i + 1;
                var b = (i + 1) % meridians + 1;
                indices.Add(0);
                indices.Add((uint)b);
                indices.Add(a);
            }

            for (int j = 0; j < parallels - 2; j++)
            {
                var ja = j * (meridians + 1) + 1;
                var jb = (j + 1) * (meridians + 1) + 1;
                for (int i = 0; i < meridians; i++)
                {
                    var a = ja + i;
                    var c = ja + (i + 1);
                    var b = jb + i;
                    var d = jb + (i + 1);

                    indices.Add((uint)a);
                    indices.Add((uint)c);
                    indices.Add((uint)b);
                    indices.Add((uint)b);
                    indices.Add((uint)c);
                    indices.Add((uint)d);
                }
            }

            for (uint i = 0; i < meridians; i++)
            {
                var a = i + meridians * (parallels - 2) + 1;
                var b = (i + 1) % meridians + meridians * (parallels - 2) + 1;
                var c = vertices.Count - 1;
                indices.Add((uint)c);
                indices.Add((uint)a);
                indices.Add((uint)b);
            }

            return new Tuple<uint[], T[]>(indices.ToArray(), vertices.ToArray());
        }

        /// <summary>
        /// Sphere with line on axis, coloried according to direction
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="meridians"></param>
        /// <param name="parallels"></param>
        /// <returns></returns>
        public static Tuple<uint[], T[]> OrientationSphere(float radius, int meridians, int parallels)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Axis with cone arrows.
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="meridians"></param>
        /// <param name="parallels"></param>
        /// <returns></returns>
        public static Tuple<uint[], T[]> Axis(float radius, int meridians, int parallels)
        {
            throw new NotImplementedException();
        }

        private static T Vertex(Vector3 position, Vector3 normal, Vector2 texture, Vector4 color)
        {
            T result = new T();

            result.Position = position;
            result.Normal = normal;
            result.Texture = texture;

            return result;
        }

        static Primitives()
        {

        }
    }
}
