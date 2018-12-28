using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Windows
{
    public class ControlCollection : List<Control>
    {
        private Control control;

        public ControlCollection(Control control)
        {
            this.control = control;
        }

        public new void Add(Control item)
        {
            item.Window = control.Window;
            base.Add(item);
        }
    }
}
