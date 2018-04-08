using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Injection;
using Axiverse.Interface.Graphics;

namespace Axiverse.Interface.Rendering
{
    public class Material
    {
        // Various parameters including textures, constants, etc.
        public readonly BindingDictionary Bindings = new BindingDictionary();
    }
}
