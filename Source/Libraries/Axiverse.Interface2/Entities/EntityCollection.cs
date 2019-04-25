using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Collections;

namespace Axiverse.Interface2.Entities
{
    public class EntityCollection : TrackedList<Entity>
    {
        private Scene Scene { get; }

        public EntityCollection(Scene scene)
        {
            Scene = scene;
        }

        protected override void OnItemAdded(Entity item)
        {
            Scene.OnEntityAdded(new EntityEventArgs(item));
        }

        protected override void OnItemRemoved(Entity item)
        {
            Scene.OnEntityRemoved(new EntityEventArgs(item));
        }
    }
}
