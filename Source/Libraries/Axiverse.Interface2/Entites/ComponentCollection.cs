using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Collections;

namespace Axiverse.Interface2.Entites
{
    public class ComponentCollection : TrackedDictionary<Type, Component>
    {
        private Entity Entity { get; }

        public ComponentCollection(Entity entity)
        {
            Entity = entity;
        }

        protected override void OnItemAdding(Type key, Component value)
        {
            Requires.That(value.Entity == null);
            Requires.That<InvalidCastException>(key.IsAssignableFrom(value.GetType()));
        }

        protected override void OnItemAdded(Type key, Component value)
        {
            value.Entity = Entity;
            Entity.OnComponentAdded(new ComponentEventArgs(key, value, Entity));
        }

        protected override void OnItemRemoved(Type key, Component value)
        {
            value.Entity = null;
            Entity.OnComponentRemoved(new ComponentEventArgs(key, value, Entity));
        }
    }
}
