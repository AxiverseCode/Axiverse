using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    public class StoreCollection
    {
        public Library Library { get; }

        public StoreCollection(Library library)
        {
            Library = library;
        }
    }
}
