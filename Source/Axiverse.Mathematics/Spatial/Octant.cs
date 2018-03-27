using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Spatial
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Octant<T> where T : class, ISpatial3
    {
        /// <summary>
        /// Gets whether this Octant has no children.
        /// </summary>
        public bool IsLeaf { get; private set; } = true;

        /// <summary>
        /// Gets the bounds of this octant.
        /// </summary>
        public Bounds3 Bounds { get; private set; }

        /// <summary>
        /// Gets the bounds of this and all neighboring nodes of the same level.
        /// </summary>
        public Bounds3 Influence { get; set; }

        /// <summary>
        /// Gets the distance from the center to the nearest face or half the length of a side.
        /// </summary>
        public float Extent { get; private set; }

        /// <summary>
        /// Gets a list of the items within the octant.
        /// </summary>
        public List<T> Items { get; } = new List<T>();

        Octant<T>[] children = new Octant<T>[8];

        /// <summary>
        /// 
        /// </summary>
        public Octant(Octant<T> parent, Bounds3 bounds)
        {
            Bounds = bounds;
            Influence = Bounds3.FromCenter(bounds.Center, bounds.Size * 3);
        }

        /// <summary>
        /// Gets whether this octant contains the specified vector.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public bool Contains(Vector3 vector)
        {
            return Bounds.Contains(vector);
        }

        public Octant<T> Add(T item)
        {
            if (item.Radius > Extent / 2 || IsLeaf)
            {
                // must fit within this level
                Items.Add(item);
                return this;
            }
            else
            {
                // can fit in children
                foreach (var child in children)
                {
                    if (child.Contains(item.Position))
                    {
                        return child.Add(item);
                    }
                }

                throw new InvalidOperationException("A child must contain the item.");
            }
        }

        public void Remove(T item)
        {
            Items.Remove(item);
        }

        public void AppendIntersecting(IList<T> list, Sphere3 sphere)
        {
            // see if the item is in the overbounds.
            foreach (var item in Items)
            {
                if (item.BoundingSphere.Intersects(sphere))
                {
                    list.Add(item);
                }
            }

            // recurse for all children that has influence over the item.
            if (!IsLeaf)
            {
                foreach (var child in children)
                {
                    if (child.Influence.Contains(sphere.Position))
                    {
                        child.AppendIntersecting(list, sphere);
                    }
                }
            }
        }

        public void AppendContaining(IList<T> list, Bounds3 bounds)
        {
            // append all items within the bounds
            foreach (var item in Items)
            {
                if (bounds.Contains(item.Position))
                {
                    list.Add(item);
                }
            }

            // recurse for all children that intersects the bounds.
            if (!IsLeaf)
            {
                foreach (var child in children)
                {
                    if (child.Bounds.Intersects(bounds))
                    {
                        child.AppendContaining(list, bounds);
                    }
                }
            }
        }

        private void CreateChildren()
        {
            if (IsLeaf)
            {
                // create each of the child octants
                for (int i = 0; i < 8; i++)
                {
                    children[i] = new Octant<T>(this, GetChildBounds(Bounds, (Boolean3)i));
                }

                IsLeaf = false;

                // relocate items which can fit in children octents
                foreach (var item in Items.FindAll(item => item.Radius <= Extent / 2))
                {
                    Items.Remove(item);
                    Add(item);
                }
            }
        }

        /// <summary>
        /// Creates the bounding box for the specified octant.
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="octant"></param>
        /// <returns></returns>
        private static Bounds3 GetChildBounds(Bounds3 bounds, Boolean3 octant)
        {
            Vector3 center = bounds.Center;
            Vector3 half = bounds.Size / 2;
            Vector3 corner = center + (half * octant.ToVector3());

            return Bounds3.FromVectors(center, corner);
        }
    }
}
