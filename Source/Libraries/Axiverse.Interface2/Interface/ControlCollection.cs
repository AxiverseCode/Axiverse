using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Collections;

namespace Axiverse.Interface2.Interface
{
    public class ControlCollection : TrackedList<Control>
    {
        private readonly Control control;
        private readonly Chrome overlay;
        
        public ControlCollection(Control control)
        {
            this.control = control;
        }

        public ControlCollection(Chrome overlay)
        {
            this.overlay = overlay;
        }

        protected override void OnItemAdded(Control item)
        {
            if (control != null)
            {
                item.Parent = control;
                item.Chrome = control.Chrome;
            }
            else if (overlay != null)
            {
                item.Chrome = overlay;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        protected override void OnItemRemoved(Control item)
        {
            item.Parent = null;
            item.Chrome = null;
        }
    }
}
