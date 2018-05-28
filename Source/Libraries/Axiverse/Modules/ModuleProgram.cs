using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Axiverse.Injection;

namespace Axiverse.Modules
{
    public static class ModuleProgram
    {
        public static void Run(string[] args)
        {
            // parse arguments and flags

            // create injector

            // load primary module(s)
            var main = Injector.Global.Resolve<Main>();
            main(args);
        }
    }
}
