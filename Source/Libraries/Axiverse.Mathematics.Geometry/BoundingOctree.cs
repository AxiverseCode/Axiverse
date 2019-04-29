using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Geometry
{
    /// <summary>
    /// Loose bounding sparse octree.
    /// </summary>
    public class BoundingOctree<T>
    {
        /// <summary>
        /// Gets the current level as a power of 2 of the <see cref="BoundingOctree{T}"/>. The 
        /// octree will span from [-(2^n), 2^n) for each component.
        /// </summary>
        public int Level { get; private set; } = int.MinValue;

        /// <summary>
        /// Gets the bounds of the <see cref="BoundingOctree{T}"/>. This is the bounary which the
        /// octree contains at the current level.
        /// </summary>
        public Bounds3 Bounds { get; private set; }

        public BoundingOctreeNode<T>[] children;

        /// <summary>
        /// Adds an item with the given bounds into the octree.
        /// </summary>
        /// <param name="bounds">The bounds of the item.</param>
        /// <param name="value">The item to add.</param>
        /// <returns></returns>
        public BoundsOctreeItem<T> Add(Bounds3 bounds, T value)
        {
            var item = new BoundsOctreeItem<T>(bounds, value);

            if (children == null)
            {
                CreateInitialOctree(item);
                return item;
            }

            if (Level < item.Level)
            {
                Expand(item);
            }

            var octant = GetOctant(bounds.Center);

            if (children[octant] == null)
            {
                children[octant] = new BoundingOctreeNode<T>(Level, GetOriginIndex3(octant));
            }

            children[octant].Add(item);
            return item;
        }

        public void Remove(BoundsOctreeItem<T> item)
        {
            item.Node.Items.Remove(item);

            // Clean up tree.
        }

        private void CreateInitialOctree(BoundsOctreeItem<T> item)
        {
            Level = item.Level;

            var side = Functions.Pow(2, Level);
            Bounds = new Bounds3(-side, -side, -side, side, side, side);

            children = new BoundingOctreeNode<T>[8];
            var octant = GetOctant(item.Bounds.Center);
            children[octant] = new BoundingOctreeNode<T>(Level, GetOriginIndex3(octant));
            children[octant].Add(item);
        }

        private void Expand(BoundsOctreeItem<T> item)
        {
            for (int l = Level; l < item.Level; l++)
            {
                Level += 1;
                for (int i = 0; i < 8; i++)
                {
                    if (children[i] != null)
                    {
                        var parent = new BoundingOctreeNode<T>(Level, children[i].Index);
                        parent.children = new BoundingOctreeNode<T>[8];
                        parent.children[GetOpposingOctant(i)] = children[i];
                        children[i] = parent;
                    }
                }
            }
        }

        public static Index3 GetOriginIndex3(int octant)
        {
            return new Index3(
                -1 + (octant & 0b001), // >> 0
                -1 + ((octant & 0b010) >> 1),
                -1 + ((octant & 0b100) >> 2));
        }

        public static int GetOctant(Vector3 vector)
        {
            int value = 0;
            value |= (vector.X >= 0) ? 1 : 0;
            value |= (vector.Y >= 0) ? 2 : 0;
            value |= (vector.Z >= 0) ? 4 : 0;
            return value;
        }

        public static int GetOpposingOctant(int octant)
        {
            return (~octant) & 0b111;
        }
    }

    public class BoundingOctreeNode<T>
    {
        private const int EdgeThreshold = 8;

        public int Level { get; }
        public Index3 Index { get; }

        public Vector3 Center { get; }
        public Bounds3 Bounds { get; }
        public Bounds3 Expansion { get; }

        public BoundingOctreeNode<T> Parent { get; internal set; }
        
        public BoundingOctreeNode<T>[] children;

        public List<BoundsOctreeItem<T>> Items { get; } = new List<BoundsOctreeItem<T>>();

        public BoundingOctreeNode(int level, Index3 index)
        {
            Level = level;
            Index = index;

            var side = Functions.Pow(2, level);
            var minimum = (Vector3)index * side;
            var maximum = minimum + new Vector3(side);

            Bounds = new Bounds3(minimum, maximum);
            Center = Bounds.Center;
            Expansion = Bounds3.Expand(Bounds, 2, Center);
        }

        public void Add(BoundsOctreeItem<T> item)
        {
            if (!Bounds.Intersects(item.Bounds))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (item.Level > Level)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (item.Level == Level)
            {
                Items.Add(item);
                return;
            }

            var octant = GetOctant(item.Bounds.Center);
            
            if (children == null)
            {
                if (Items.Count < EdgeThreshold)
                {
                    // Add and continue
                    Items.Add(item);
                    return;
                }

                // Fork children
                children = new BoundingOctreeNode<T>[8];

                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    // Find the target level, if lower, add to child.
                    if (Items[i].Level < Level)
                    {
                        var removed = Items[i];
                        Items.RemoveAt(i);
                        Add(removed);
                    }
                }
            }

            if (children[octant] == null)
            {
                children[octant] = new BoundingOctreeNode<T>(Level - 1, GetChildIndex(octant));
            }

            children[octant].Add(item);
        }

        public Index3 GetChildIndex(int octant)
        {
            return new Index3(
                Index.A * 2 + ((octant & 0b001) == 0 ? 0 : 1),
                Index.B * 2 + ((octant & 0b010) == 0 ? 0 : 1),
                Index.C * 2 + ((octant & 0b100) == 0 ? 0 : 1)
                );
        }

        public int GetOctant(Vector3 vector)
        {
            return BoundingOctree<T>.GetOctant(vector - Center);
        }

        public override string ToString()
        {
            return $"{Index} @ {Level} {Bounds}";
        }
    }

    public class BoundsOctreeItem<T>
    {
        public BoundingOctreeNode<T> Node { get; internal set; }
        public Bounds3 Bounds { get; internal set; }
        public int Level { get; internal set; }
        public T Value { get; }

        public BoundsOctreeItem(Bounds3 bounds, T value)
        {
            Bounds = bounds;
            Value = value;
            Level = Math.Max(FindLevelBySize(bounds), FindLevelByPosition(bounds));
            Node = null;
        }

        private int FindLevelBySize(Bounds3 bounds)
        {
            return (int)Math.Ceiling(Functions.Log2(Functions.MaximumComponent(bounds.Size)));
        }

        private int FindLevelByPosition(Bounds3 bounds)
        {
            var furthestMax = Functions.MaximumComponent(Functions.Abs(bounds.Maximum));
            var furthestMin = Functions.MaximumComponent(Functions.Abs(bounds.Minimum));

            return (int)Math.Ceiling(Functions.Log2(Math.Max(furthestMax, furthestMin)));
        }
    }
}
