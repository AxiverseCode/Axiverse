using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Game
{
    /// <summary>
    /// Collection of <see cref="GameSystem"/>.
    /// </summary>
    public class GameSystemCollection : List<GameSystem>
    {
        public void Update(GameContext context)
        {
            foreach(var system in this)
            {
                system.Update(context);
            }
        }

        public void Render(GameContext context)
        {
            foreach (var system in this)
            {
                system.Render(context);
            }
        }
    }
}
