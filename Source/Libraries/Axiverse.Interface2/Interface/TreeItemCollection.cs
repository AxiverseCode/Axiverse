using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Interface
{
    public class TreeItemCollection : IEnumerable<TreeItem>
    {
        private readonly List<TreeItem> items = new List<TreeItem>();

        public TreeItem this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }

        public int Count => items.Count;

        public void Add(TreeItem item)
        {
            items.Add(item);
        }

        public IEnumerator<TreeItem> GetEnumerator()
        {
            return ((IEnumerable<TreeItem>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
