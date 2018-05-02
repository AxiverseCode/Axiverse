using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Axiverse.Interface;
using Axiverse.Resources;

namespace Axiverse.Examples.HelloWorld
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // setup resource filesystem.
            Store.Default.Mount(new Mount(@"..\..\..\..\Resources" ));

            // initialize the engine.
            var engine = new Engine2();

            engine.Initialize();
            engine.Start();
        }
    }
}
