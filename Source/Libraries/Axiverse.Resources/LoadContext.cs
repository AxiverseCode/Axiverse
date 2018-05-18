using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Resources
{
    public struct LoadContext
    {
        public Library Library { get; }

        public string Path { get; }

        public LoadContext(Library library, string path)
        {
            Library = library;
            Path = path;
        }
    }
}
