using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Interface.Game;

namespace Axiverse.Interface.Scene
{
    public class SceneSystem : GameSystem
    {
        public EntityCollection Entities { get; } = new EntityCollection();

        public ProcessorCollection Processors { get; } = new ProcessorCollection();

        public Entity Root { get; set; }

        public override void Update(GameContext context)
        {
            foreach(var processor in Processors)
            {
                processor.Process();
            }
        }
    }
}
