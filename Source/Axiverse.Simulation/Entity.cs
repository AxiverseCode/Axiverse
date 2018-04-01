using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Simulation
{
    /// <summary>
    /// An entity defined by the attributes in its set of components.
    /// </summary>
    public class Entity
    {
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
        public ComponentCollection Components { get; }

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
        /// Constructs an entity with the given identifier.
        /// </summary>
        /// <param name="identifier"></param>
        public Entity(Guid identifier)
        {
            Identifier =identifier;
            Components = new ComponentCollection();
            Spatial = new SpatialComponent();

            Components[typeof(SpatialComponent)] = Spatial;
        }

        /// <summary>
        /// Sets the component bound to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : Component
        {
            return Components[typeof(T)] as T;
        }

        /// <summary>
        /// Gets a component bound to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        public void Set<T>(T component) where T : Component
        {
            Components[typeof(T)] = component;
        }
    }
}
