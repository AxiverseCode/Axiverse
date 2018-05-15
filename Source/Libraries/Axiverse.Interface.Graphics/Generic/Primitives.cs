using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Vertex( new Vector3(-1.0f, -1.0f,  1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f), new Vector4(1.0f, 0.0f, 0.0f, 0.0f) ),
                Vertex( new Vector3(-1.0f,  1.0f,  1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 0.0f) ),
                Vertex( new Vector3( 1.0f,  1.0f,  1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 0.0f) ),
                Vertex( new Vector3( 1.0f, -1.0f,  1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector4(1.0f, 0.0f, 0.0f, 0.0f) ),

                // back
                Vertex( new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f), new Vector4(0.0f, 1.0f, 0.0f, 0.0f) ),
                Vertex( new Vector3( 1.0f, -1.0f, -1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 0.0f) ),
                Vertex( new Vector3( 1.0f,  1.0f, -1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 0.0f) ),
                Vertex( new Vector3(-1.0f,  1.0f, -1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector4(0.0f, 1.0f, 0.0f, 0.0f) ),

                // top
                Vertex( new Vector3(-1.0f,  1.0f, -1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f), new Vector4(0.0f, 0.0f, 1.0f, 0.0f) ),
                Vertex( new Vector3( 1.0f,  1.0f, -1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 0.0f) ),
                Vertex( new Vector3( 1.0f,  1.0f,  1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 0.0f) ),
                Vertex( new Vector3(-1.0f,  1.0f,  1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector4(0.0f, 0.0f, 1.0f, 0.0f) ),

                // bottom
                Vertex( new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f) ),
                Vertex( new Vector3(-1.0f, -1.0f,  1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 1.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f) ),
                Vertex( new Vector3( 1.0f, -1.0f,  1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f) ),
                Vertex( new Vector3( 1.0f, -1.0f, -1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f) ),

                // right
                Vertex( new Vector3( 1.0f, -1.0f, -1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f), new Vector4(1.0f, 1.0f, 0.0f, 0.0f) ),
                Vertex( new Vector3( 1.0f, -1.0f,  1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 1.0f), new Vector4(1.0f, 1.0f, 0.0f, 0.0f) ),
                Vertex( new Vector3( 1.0f,  1.0f,  1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f), new Vector4(1.0f, 1.0f, 0.0f, 0.0f) ),
                Vertex( new Vector3( 1.0f,  1.0f, -1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector4(1.0f, 1.0f, 0.0f, 0.0f) ),

                // left
                Vertex( new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f), new Vector4(1.0f, 0.0f, 1.0f, 0.0f) ),
                Vertex( new Vector3(-1.0f,  1.0f, -1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 0.0f) ),
                Vertex( new Vector3(-1.0f,  1.0f,  1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 0.0f) ),
                Vertex( new Vector3(-1.0f, -1.0f,  1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector4(1.0f, 0.0f, 1.0f, 0.0f) ),
            };

            return new Tuple<uint[], T[]>(indices, vertices);
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
