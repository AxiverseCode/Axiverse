using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources.New
{
    public abstract class Node
    {
        public abstract bool IsDirectory { get; }

        public abstract bool IsBuffer { get; }

        public abstract bool IsLink { get; }
    }
}
