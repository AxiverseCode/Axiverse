using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse
{
    public struct Revision
    {
        private int revision;

        public void Reset()
        {

        }

        public void Dirty()
        {

        }
    }

    public struct Dependency
    {
        private uint hash;
    }
}
