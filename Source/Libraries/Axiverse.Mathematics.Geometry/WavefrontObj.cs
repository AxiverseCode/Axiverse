using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    public static class WavefrontObj
    {
        public static Mesh Load(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                return Load(stream);
            }
        }
        public static Mesh Load(Stream stream, bool optimize = false)
        {
            StreamReader reader = new StreamReader(stream);

            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> textures = new List<Vector2>();

            List<Index3[]> faces = new List<Index3[]>();
            HashSet<Index3> indices = new HashSet<Index3>();

            // Read the obj file.
            int lineNumber = 0;
            while (!reader.EndOfStream)
            {
                lineNumber++;

                var line = reader.ReadLine();
                var segments = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (segments.Length == 0)
                {
                    continue;
                }

                switch(segments[0])
                {
                    case "v": vertices.Add(ParseVector3(segments, lineNumber)); break;
                    case "vt": textures.Add(ParseVector2(segments, lineNumber)); break;
                    case "vn": normals.Add(ParseVector3(segments, lineNumber)); break;
                    case "f": faces.Add(ParseFace(segments, indices, lineNumber)); break;
                    case "#":
                    default:
                        break;
                }
            }

            Mesh mesh = new Mesh();
            Func<Index3, int> addVertex = index =>
            {
                Vertex vertex = new Vertex
                {
                    Color = new Vector4(1, 1, 1, 1),
                    Position = (index.A >= 0) ? vertices[index.A] : default,
                    Texture = (index.B >= 0) ? textures[index.B] : default,
                    Normal = (index.C >= 0) ? normals[index.C] : default
                };

                mesh.Vertices.Add(vertex);
                return mesh.Vertices.Count - 1;
            };

            if (optimize)
            {
                // Look for unique mappings.
                Dictionary<Index3, int> indexMapping = new Dictionary<Index3, int>();
                foreach (var index in indices)
                {
                    // Save the index of the mapping and add the vertex.
                    indexMapping[index] = addVertex(index);
                }

                // Create the faces by triangulation.
                foreach (var face in faces)
                {
                    for (int i = 0; i < face.Length - 2; i++)
                    {
                        mesh.Triangles.Add(new Index3
                        {
                            A = indexMapping[face[0]],
                            B = indexMapping[face[i + 1]],
                            C = indexMapping[face[i + 2]]
                        });
                    }
                }
            }
            else
            {
                foreach (var face in faces)
                {
                    int offset = mesh.Vertices.Count;
                    for (int i = 0; i < face.Length; i++)
                    {
                        addVertex(face[i]);
                    }

                    for (int i = 0; i < face.Length - 2; i++)
                    {
                        mesh.Triangles.Add(new Index3
                        {
                            A = offset,
                            B = offset + i + 1,
                            C = offset + i + 2
                        });
                    }
                }
            }

            return mesh;
        }

        private static Vector2 ParseVector2(string[] segments, int lineNumber)
        {
            Requires.That(segments.Length > 2);
            return new Vector2(
                ReadFloat(segments[1], "X", lineNumber),
                ReadFloat(segments[2], "Y", lineNumber));
        }
        private static Vector3 ParseVector3(string[] segments, int lineNumber)
        {
            Requires.That(segments.Length > 3);
            return new Vector3(
                ReadFloat(segments[1], "X", lineNumber),
                ReadFloat(segments[2], "Y", lineNumber),
                ReadFloat(segments[3], "Z", lineNumber));
        }

        private static Index3[] ParseFace(string[] segments, HashSet<Index3> indices, int lineNumber)
        {
            Index3[] faceIndices = new Index3[segments.Length - 1];

            for (int i = 0; i < faceIndices.Length; i++)
            {
                faceIndices[i] = NormalizeIndex(segments[i + 1], i, lineNumber);
                indices.Add(faceIndices[i]);
            }

            return faceIndices;
        }

        private static Index3 NormalizeIndex(string index, int indexNumber, int lineNumber)
        {
            Index3 value = default;
            var segments = index.Split('/');

            if (segments.Length <= 0 ||
                (segments[0] != "" && !int.TryParse(segments[0], out value.A)))
            {
                throw new ArgumentException($"Could not parse position on for index {indexNumber} on line {lineNumber}");
            }

            if (segments.Length <= 1 ||
                (segments[1] != "" && !int.TryParse(segments[1], out value.B)))
            {
                throw new ArgumentException($"Could not parse texture on for index {indexNumber} line {lineNumber}");
            }

            if (segments.Length <= 2 ||
                (segments[2] != "" && !int.TryParse(segments[2], out value.C)))
            {
                throw new ArgumentException($"Could not parse normal on for index {indexNumber} line {lineNumber}");
            }

            value.A--;
            value.B--;
            value.C--;
            return value;
        }

        private static float ReadFloat(string value, string parameter, int lineNumber)
        {
            if (!float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
            {
                throw new ArgumentException($"Could not parse parameter {parameter} on line {lineNumber}");
            }
            return number;
        }

        public static void Save(Mesh mesh, string path)
        {
            using (var stream = File.OpenWrite(path))
            {
                Save(mesh, stream);
            }
        }

        public static void Save(Mesh mesh, Stream stream)
        {
            // TODO: use indexing.

            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("# Axiverse engine obj. Version 0.1");

            foreach (var vertex in mesh.Vertices)
            {
                writer.WriteLine($"v {vertex.Position.X} {vertex.Position.Y} {vertex.Position.Z}");
            }

            foreach (var vertex in mesh.Vertices)
            {
                writer.WriteLine($"vt {vertex.Texture.X} {vertex.Texture.Y}");
            }

            foreach (var vertex in mesh.Vertices)
            {
                writer.WriteLine($"vn {vertex.Normal.X} {vertex.Normal.Y} {vertex.Normal.Z}");
            }

            foreach (var t in mesh.Triangles)
            {
                writer.WriteLine($"f {t.A}/{t.A}/{t.A} {t.B}/{t.B}/{t.B} {t.C}/{t.C}/{t.C}");
            }

            foreach (var q in mesh.Quadrilaterals)
            {
                writer.WriteLine($"f {q.A}/{q.A}/{q.A} {q.B}/{q.B}/{q.B} {q.C}/{q.C}/{q.C} {q.D}/{q.D}/{q.D}");
            }
        }
    }
}
