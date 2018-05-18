using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Resources;
using Axiverse.Injection;
using Axiverse.Interface.Graphics;
using Axiverse.Interface.Engine;
using Axiverse.Interface.Rendering;
using System.IO;

namespace Axiverse.Interface.Assets.Models
{
    public class ObjLoader : ILoader<Model>
    {
        public string[] Extensions => new String[] { "obj" };

        public Model Load(Stream stream, LoadContext context)
        {
            throw new NotImplementedException();
        }
    }
}
