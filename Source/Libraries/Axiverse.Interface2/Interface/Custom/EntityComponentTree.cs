using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface2.Entities;

namespace Axiverse.Interface2.Interface.Custom
{
    public class EntityComponentTree : Tree
    {
        private Dictionary<Entity, TreeItem> entites = new Dictionary<Entity, TreeItem>();
        private Dictionary<Component, TreeItem> components = new Dictionary<Component, TreeItem>();

        public EntityComponentTree(Scene scene)
        {
            scene.EntityAdded += HandleEntityAdded;
            scene.ComponentAdded += HandleComponentAdded;

            foreach (var entity in scene.Entities)
            {
                var entityItem = new TreeItem(entity.Name);
                Items.Add(entityItem);
                entites.Add(entity, entityItem);

                foreach (var component in entity.Components)
                {
                    var componentItem = new TreeItem(component.Key.Name);
                    entityItem.Children.Add(componentItem);
                    components.Add(component.Value, componentItem);
                }
            }
        }

        private void HandleEntityAdded(object sender, EntityEventArgs args)
        {
            var entityItem = new TreeItem(args.Entity.Name);
            Items.Add(entityItem);
            entites.Add(args.Entity, entityItem);

            foreach (var component in args.Entity.Components)
            {
                var componentItem = new TreeItem(component.Key.Name);
                entityItem.Children.Add(componentItem);
                components.Add(component.Value, componentItem);
            }
        }

        private void HandleComponentAdded(object sender, ComponentEventArgs args)
        {
            var entityItem = entites[args.Entity];

            var componentItem = new TreeItem(args.Binding.Name);
            entityItem.Children.Add(componentItem);
            components.Add(args.Component, componentItem);
        }
    }
}
