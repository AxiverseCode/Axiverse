using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Collections;

namespace Axiverse.Interface2.Interface
{
    public class MenuItemCollection : TrackedList<MenuItem>
    {
        public void Add(string text)
        {
            Add(new MenuItem(text));
        }

        public void AddRange(params string[] collection)
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        protected override void OnItemAdded(MenuItem item)
        {

        }

        protected override void OnItemRemoved(MenuItem item)
        {

        }
    }
}
