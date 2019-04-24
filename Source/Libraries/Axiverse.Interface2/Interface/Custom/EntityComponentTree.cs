using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Entites;

namespace Axiverse.Interface2.Interface.Custom
{
    public class EntityComponentTree : Tree
    {
        private Dictionary<Entity, TreeItem> entites = new Dictionary<Entity, TreeItem>();
        private Dictionary<Component, TreeItem> components = new Dictionary<Component, TreeItem>();

        public EntityComponentTree(Scene scene)
        {
            foreach (var entity in scene.Entities)
            {
                var entityItem = new TreeItem(entity.Name);
                Items.Add(entityItem);

                foreach (var component in entity.Components)
                {
                    var componentItem = new TreeItem(component.Key.Name);
                    entityItem.Children.Add(componentItem);
                }
            }   
        }
    }
}
