using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Interface.Game;

namespace Axiverse.Interface.Compositing
{
    public class CompositingSystem : GameSystem
    {
        public override void Render(GameContext context)
        {
            // select things from scene system

            // render each stage sequentially
            {
                // Feature sets up the shaders and all the resource, extracts the data, prepares, then draws.
                // Pipeline processor create per object state in a cbuffer array, etc.
                // Renderers do the actual rendering. of a specific type of object Can take in a object, renders it.
            }
        }
    }
}
