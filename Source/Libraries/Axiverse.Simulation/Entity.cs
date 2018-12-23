using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Simulation.Components;

namespace Axiverse.Simulation
{
    /// <summary>
    /// An entity defined by the attributes in its set of components.
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Gets the universe which this entity is attached to.
        /// </summary>
        public Universe Universe { get; internal set; }

        /// <summary>
        /// Gets if this entity is attached to a universe.
        /// </summary>
        public bool IsAttached => Universe != null;

        /// <summary>
        /// Gets the identifier for this entity.
        /// </summary>
        public Guid Identifier { get; }

        /// <summary>
        /// Gets or sets the name for this entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the collection of components in this entity.
        /// </summary>
        public ComponentDictionary Components { get; }

        /// <summary>
        /// Gets the spatial component of this entity.
        /// </summary>
        public SpatialComponent Spatial { get; }

        /// <summary>
        /// Constructs an entity with a new identifier.
        /// </summary>
        public Entity() : this(Guid.NewGuid())
        {
        }

        /// <summary>
        /// Constructs an entity with a new identifier.
        /// </summary>
        public Entity(string name) : this()
        {
            Name = name;
        }

        /// <summary>
        /// Constructs an entity with the given identifier.
        /// </summary>
        /// <param name="identifier"></param>
        public Entity(Guid identifier)
        {
            Identifier = identifier;
            Components = new ComponentDictionary(this);
            Spatial = new SpatialComponent();
            Components.Add(Spatial);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Entity Clone() => Clone(Guid.NewGuid());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public Entity Clone(Guid identifier)
        {
            var entity = new Entity(identifier);

            return entity;
        }

        public T GetComponent<T>()
            where T : Component
        {
            return Components.Get<T>();
        }

        /// <summary>
        /// String representation of the entity.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return $"Entity [{ Identifier.ToString().Substring(0, 8) }] @ { Spatial.Position }";
            }
            return $"{Name} @ { Spatial.Position }";
        }

        /// <summary>
        /// Invokes when a component is added.
        /// </summary>
        /// <param name="e"></param>
        protected internal void OnComponentAdded(ComponentEventArgs e)
        {
            ComponentAdded?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes when a component is removed.
        /// </summary>
        /// <param name="e"></param>
        protected internal void OnComponentRemoved(ComponentEventArgs e)
        {
            ComponentRemoved?.Invoke(this, e);
        }

        /// <summary>
        /// Event when a component is added.
        /// </summary>
        public event ComponentEventHandler ComponentAdded;

        /// <summary>
        /// Event when a component is removed.
        /// </summary>
        public event ComponentEventHandler ComponentRemoved;
    }
}
