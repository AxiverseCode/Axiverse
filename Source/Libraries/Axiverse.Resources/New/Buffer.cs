using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources.New
{
    public abstract class Buffer : Node
    {
        public override bool IsDirectory => false;

        public override bool IsBuffer => true;

        public override bool IsLink => false;

        public abstract Stream Open();
    }
}
