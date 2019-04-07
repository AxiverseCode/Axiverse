using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources.New
{
    public class Directory : Node
    {
        public override bool IsDirectory => true;

        public override bool IsBuffer => false;

        public override bool IsLink => false;
    }
}
