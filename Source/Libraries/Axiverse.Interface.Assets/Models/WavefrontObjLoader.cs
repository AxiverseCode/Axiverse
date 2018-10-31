using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Resources;

namespace Axiverse.Interface.Graphics
{
    public class WavefrontObjLoader
    {
        public List<Vertex> VertexList;
        public List<Face> FaceList;
        public List<TextureVertex> TextureList;

        public int TriangleCount;

        public string UseMtl { get; set; }
        public string Mtl { get; set; }

        public WavefrontObjLoader()
        {
            VertexList = new List<Vertex>();
            FaceList = new List<Face>();
            TextureList = new List<TextureVertex>();
        }

        public void LoadObj(string path)
        {
            LoadObj(Store.Default.Open(path, FileMode.Open));
        }

        public void LoadObj(Stream data)
        {
            using (var reader = new StreamReader(data))
            {
                LoadObj(reader.ReadToEnd().Split(Environment.NewLine.ToCharArray()));
            }
        }

        public void LoadObj(IEnumerable<string> data)
        {
            foreach (var line in data)
            {
                ProcessLine(line);
            }
        }

        private void ProcessLine(string line)
        {
            string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 0)
            {
                switch (parts[0])
                {
                    case "usemtl":
                        UseMtl = parts[1];
                        break;
                    case "mtllib":
                        Mtl = parts[1];
                        break;
                    case "v":
                        Vertex v = new Vertex();
                        v.LoadFromStringArray(parts);
                        VertexList.Add(v);
                        v.Index = VertexList.Count();
                        break;
                    case "vn":
                        break;
                    case "s":
                        break;
                    case "f":
                        Face f = new Face();
                        f.LoadFromStringArray(parts);
                        f.UseMtl = UseMtl;
                        FaceList.Add(f);
                        TriangleCount += f.VertexIndexList.Length - 2;
                        break;
                    case "vt":
                        TextureVertex vt = new TextureVertex();
                        vt.LoadFromStringArray(parts);
                        TextureList.Add(vt);
                        vt.Index = TextureList.Count();
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine(line);
                        break;
                }
            }
        }

        interface IType
        {
            void LoadFromStringArray(string[] data);
        }

        public class Vertex : IType
        {
            public const int MinimumDataLength = 4;
            public const string Prefix = "v";

            public double X { get; set; }

            public double Y { get; set; }

            public double Z { get; set; }

            public int Index { get; set; }

            public Vector3 Vector;

            public void LoadFromStringArray(string[] data)
            {
                if (data.Length < MinimumDataLength)
                    throw new ArgumentException("Input array must be of minimum length " + MinimumDataLength, "data");

                if (!data[0].ToLower().Equals(Prefix))
                    throw new ArgumentException("Data prefix must be '" + Prefix + "'", "data");

                bool success;

                double x, y, z;

                success = double.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                if (!success) throw new ArgumentException("Could not parse X parameter as double");

                success = double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                if (!success) throw new ArgumentException("Could not parse Y parameter as double");

                success = double.TryParse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture, out z);
                if (!success) throw new ArgumentException("Could not parse Z parameter as double");

                X = x;
                Y = y;
                Z = z;

                Vector = new Vector3((float)x, (float)y, (float)z);
            }

            public override string ToString()
            {
                return string.Format("v {0} {1} {2}", X, Y, Z);
            }
        }

        public class Face : IType
        {
            public const int MinimumDataLength = 4;
            public const string Prefix = "f";

            public string UseMtl { get; set; }
            public int[] VertexIndexList { get; set; }
            public int[] TextureVertexIndexList { get; set; }

            public void LoadFromStringArray(string[] data)
            {
                if (data.Length < MinimumDataLength)
                    throw new ArgumentException("Input array must be of minimum length " + MinimumDataLength, "data");

                if (!data[0].ToLower().Equals(Prefix))
                    throw new ArgumentException("Data prefix must be '" + Prefix + "'", "data");

                int vcount = data.Count() - 1;
                VertexIndexList = new int[vcount];
                TextureVertexIndexList = new int[vcount];

                bool success;

                for (int i = 0; i < vcount; i++)
                {
                    string[] parts = data[i + 1].Split('/');

                    int vindex;
                    success = int.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out vindex);
                    if (!success) throw new ArgumentException("Could not parse parameter as int");
                    VertexIndexList[i] = vindex;

                    if (parts.Count() > 1)
                    {
                        success = int.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out vindex);
                        if (success)
                        {
                            TextureVertexIndexList[i] = vindex;
                        }
                    }
                }
            }

            // HACKHACK this will write invalid files if there are no texture vertices in
            // the faces, need to identify that and write an alternate format
            public override string ToString()
            {
                StringBuilder b = new StringBuilder();
                b.Append("f");

                for (int i = 0; i < VertexIndexList.Count(); i++)
                {
                    if (i < TextureVertexIndexList.Length)
                    {
                        b.AppendFormat(" {0}/{1}", VertexIndexList[i], TextureVertexIndexList[i]);
                    }
                    else
                    {
                        b.AppendFormat(" {0}", VertexIndexList[i]);
                    }
                }

                return b.ToString();
            }
        }

        public class Color : IType
        {
            public float r { get; set; }
            public float g { get; set; }
            public float b { get; set; }

            public Color()
            {
                this.r = 1f;
                this.g = 1f;
                this.b = 1f;
            }

            public void LoadFromStringArray(string[] data)
            {
                if (data.Length != 4) return;
                r = float.Parse(data[1]);
                g = float.Parse(data[2]);
                b = float.Parse(data[3]);
            }

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", r, g, b);
            }
        }

        public class TextureVertex : IType
        {
            public const int MinimumDataLength = 3;
            public const string Prefix = "vt";

            public double X { get; set; }

            public double Y { get; set; }

            public int Index { get; set; }

            public Vector2 Vector;

            public void LoadFromStringArray(string[] data)
            {
                if (data.Length < MinimumDataLength)
                    throw new ArgumentException("Input array must be of minimum length " + MinimumDataLength, "data");

                if (!data[0].ToLower().Equals(Prefix))
                    throw new ArgumentException("Data prefix must be '" + Prefix + "'", "data");

                bool success;

                double x, y;

                success = double.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                if (!success) throw new ArgumentException("Could not parse X parameter as double");

                success = double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                if (!success) throw new ArgumentException("Could not parse Y parameter as double");
                X = x;
                Y = 1 - y;

                Vector = new Vector2((float)X, (float)Y);
            }

            public override string ToString()
            {
                return string.Format("vt {0} {1}", X, Y);
            }
        }
    }
}
