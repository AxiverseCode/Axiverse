using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Game
{
    /// <summary>
    /// A subsystem of the game (e.g. AI, animation, input, systems, graphics, physics, networking).
    /// </summary>
    public class GameSystem
    {
        public virtual void Update(GameContext context)
        {

        }

        public virtual void Render(GameContext context)
        {

        }
    }
}
