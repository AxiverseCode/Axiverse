using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Game
{
    /// <summary>
    /// Represents a game
    /// </summary>
    public class Game
    {
        public GameSystemCollection GameSystems { get; } = new GameSystemCollection();

        public GameContext GameContext { get; set; }

        public bool Running { get; set; }

        public Game()
        {
            // input system

            // scene system

            //
        }

        public void Run()
        {
            // set up everything before running
            BeginRun();

            while (Running)
            {
                // advance the individual frame
                Advance();
            }

            // tear down and clean up resources
            EndRun();
        }

        protected virtual void BeginRun()
        {
            Running = true;
        }

        protected virtual void EndRun()
        {

        }

        /// <summary>
        /// Advances the game a single step.
        /// </summary>
        public void Advance()
        {
            // compute the new game context and the changes in time

            // iterate through all the game systems and advance each one sequentially
            GameSystems.Update(GameContext);

            GameSystems.Render(GameContext);
        }
    }
}
