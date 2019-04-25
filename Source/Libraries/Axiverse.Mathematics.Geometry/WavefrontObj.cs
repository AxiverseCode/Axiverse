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
                    case "f": faces.AddRange(ParseFace(segments, indices, lineNumber)); break;
                    case "#":
                    default:
                        break;
                }
            }

            Mesh mesh = new Mesh();

            if (optimize)
            {
                // Look for unique mappings.
                Dictionary<Index3, int> indexMapping = new Dictionary<Index3, int>();
                foreach (var index in indices)
                {
                    Vertex vertex = default;
                    vertex.Color = new Vector4(1, 1, 1, 1);

                    if (index.A >= 0)
                    {
                        vertex.Position = vertices[index.A];
                    }

                    if (index.B >= 0)
                    {
                        vertex.Texture = textures[index.B];
                    }

                    if (index.C >= 0)
                    {
                        vertex.Normal = normals[index.C];
                    }

                    // Save the index of the mapping and add the vertex.
                    indexMapping[index] = mesh.Vertices.Count;
                    mesh.Vertices.Add(vertex);
                }

                // Create the faces.
                foreach (var face in faces)
                {
                    if (face.Length == 3)
                    {
                        mesh.Triangles.Add(new Index3
                        {
                            A = indexMapping[face[0]],
                            B = indexMapping[face[1]],
                            C = indexMapping[face[2]]
                        });
                    }
                    else
                    {
                        mesh.Quadrilaterals.Add(new Index4
                        {
                            A = indexMapping[face[0]],
                            B = indexMapping[face[1]],
                            C = indexMapping[face[2]],
                            D = indexMapping[face[3]]
                        });
                    }
                }
            }
            else
            {
                Func<Index3, int> add = i =>
                {
                    Vertex vertex = default;
                    vertex.Color = new Vector4(1, 1, 1, 1);

                    if (i.A >= 0)
                    {
                        vertex.Position = vertices[i.A];
                    }

                    if (i.B >= 0)
                    {
                        vertex.Texture = textures[i.B];
                    }

                    if (i.C >= 0)
                    {
                        vertex.Normal = normals[i.C];
                    }

                    mesh.Vertices.Add(vertex);
                    return mesh.Vertices.Count - 1;
                };
                // Create the faces.
                foreach (var face in faces)
                {
                    if (face.Length == 3)
                    {
                        mesh.Triangles.Add(new Index3
                        {
                            A = add(face[0]),
                            B = add(face[1]),
                            C = add(face[2]),
                        });
                    }
                    else
                    {
                        mesh.Quadrilaterals.Add(new Index4
                        {
                            A = add(face[0]),
                            B = add(face[1]),
                            C = add(face[2]),
                            D = add(face[3]),
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

        private static Index3[][] ParseFace(string[] segments, HashSet<Index3> indices, int lineNumber)
        {
            Index3[] faceIndices = new Index3[segments.Length - 1];
            for (int i = 0; i < faceIndices.Length; i++)
            {
                faceIndices[i] = NormalizeIndex(segments[i + 1], i, lineNumber);
                indices.Add(faceIndices[i]);
            }

            if (segments.Length > 3 && segments.Length <= 4)
            {
                // Single triangle or quad.
                return new Index3[][] { faceIndices };
            }

            // Higher order polygon. Must triangulate.
            var triangles = new Index3[faceIndices.Length - 2][];
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = new Index3[] { faceIndices[0], faceIndices[i + 1], faceIndices[i + 2] };
            }
            return triangles;
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
                (segments[2] == "" && !int.TryParse(segments[2], out value.C)))
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
