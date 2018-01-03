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
            FileSystem.Default.Mount(new Mount() { BasePath = @"..\..\..\..\Resources" });

            // initialize the engine.
            var engine = new Engine();

            engine.Initialize();
            engine.Start();
        }
    }
}
