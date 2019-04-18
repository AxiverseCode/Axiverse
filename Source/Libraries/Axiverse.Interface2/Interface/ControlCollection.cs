using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface2.Interface
{
    public class ControlCollection : IEnumerable<Control>
    {
        private readonly Control control;
        private readonly Chrome overlay;
        private readonly List<Control> items = new List<Control>();
        
        public Control this[int index]
        {
            get
            {
                return items[index];
            }
        }

        public int Count => items.Count;

        public ControlCollection(Control control)
        {
            this.control = control;
        }

        public ControlCollection(Chrome overlay)
        {
            this.overlay = overlay;
        }

        public void Add(Control item)
        {
            if (items.Contains(control))
            {
                return;
            }

            if (item.Parent != null)
            {
                item.Parent.Children.Remove(item);
            }

            items.Add(item);
            Register(item);
        }

        public bool Remove(Control item)
        {
            if (items.Remove(item))
            {
                Unregister(item);
                return true;
            }
            return false;
        }

        private void Register(Control item)
        {
            if (control != null)
            {
                item.Parent = control;
                item.Overlay = control.Overlay;
            }
            else if (overlay != null)
            {
                item.Overlay = overlay;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private void Unregister(Control item)
        {
            item.Parent = null;
            item.Overlay = null;
        }

        public IEnumerator<Control> GetEnumerator()
        {
            return ((IEnumerable<Control>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
