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
    public class Octree<T> where T : class, ISpatial3
    {
        public Bounds3 Bounds { get; }

        public Octant<T> Root { get; }

        public Octree(Vector3 center, float side)
        {
            Bounds = Bounds3.FromCenter(center, new Vector3(side));
            Root = new Octant<T>(null, Bounds);
        }

        public void Add(T item)
        {
            item.PositionChanged += HandlePositionChanged;
            var octant = Root.Add(item);
            items.Add(item, octant);
        }

        public void Remove(T item)
        {
            item.PositionChanged -= HandlePositionChanged;
            items[item].Remove(item);
        }

        public List<T> GetIntersecting(Sphere3 sphere)
        {
            var list = new List<T>();
            Root.AppendIntersecting(list, sphere);
            return list;
        }

        public List<T> GetContaining(Bounds3 bounds)
        {
            var list = new List<T>();
            Root.AppendContaining(list, bounds);
            return list;
        }

        private void HandlePositionChanged(object sender, EventArgs args)
        {
            var item = sender as T;
            items[item].Remove(item);
            var octant = Root.Add(item);
            items.Add(item, octant);
        }

        private readonly Dictionary<T, Octant<T>> items = new Dictionary<T, Octant<T>>();
    }
}
