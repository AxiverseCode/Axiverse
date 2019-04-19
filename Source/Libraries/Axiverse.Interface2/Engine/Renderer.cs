using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Entites;

namespace Axiverse.Interface2.Engine
{
    public class Renderer : IDisposable
    {
        public virtual void Render(Renderable renderable, CompositingContext context)
        {

        }

        public virtual void Dispose()
        {

        }
    }
}
