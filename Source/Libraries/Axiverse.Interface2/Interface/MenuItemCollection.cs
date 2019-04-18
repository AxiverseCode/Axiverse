using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Interface
{
    public class MenuItemCollection : IEnumerable<MenuItem>
    {
        private readonly List<MenuItem> items = new List<MenuItem>();

        public MenuItem this[int index] => items[index];
        public int Count => items.Count;

        public void Add(MenuItem item)
        {
            items.Add(item);
            OnItemAdded(item);
        }

        public bool Remove(MenuItem item)
        {
            var result = items.Remove(item);
            OnItemRemoved(item);
            return result;
        }

        protected virtual void OnItemAdded(MenuItem item)
        {

        }

        protected virtual void OnItemRemoved(MenuItem item)
        {

        }

        public IEnumerator<MenuItem> GetEnumerator()
        {
            return ((IEnumerable<MenuItem>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
