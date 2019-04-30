using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Models;
using Axiverse.Mathematics.Geometry;
using Mesh = Axiverse.Mathematics.Geometry.Mesh;

namespace Axiverse.Interface2.Assets
{
    public class Loader : IDisposable
    {
        public Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        public Dictionary<string, Model> Models = new Dictionary<string, Model>();
        public Device Device { get; }

        public Loader(Device device)
        {
            Device = device;
        }

        public Texture2D LoadTexture(string path)
        {
            if (Textures.TryGetValue(path, out var value))
            {
                return value;
            }

            value = Texture2D.FromFile(Device, path);
            Textures.Add(path, value);
            return value;
        }

        public Model LoadModel(string path)
        {
            if (Models.TryGetValue(path, out var value))
            {
                return value;
            }

            var mesh = WavefrontObj.Load(path);
            value = Model.FromMesh(Device, mesh);
            Models.Add(path, value);
            return value;
        }

        public void Dispose()
        {
            foreach (var item in Textures.Values)
            {
                item.Dispose();
            }
            Textures.Clear();

            foreach (var item in Models.Values)
            {
                item.Dispose();
            }
            Models.Clear();
        }
    }
}
