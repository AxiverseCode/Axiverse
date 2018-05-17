using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiverse.Interface.Engine;

namespace HelloGraphics
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine engine = new Engine();
            engine.Initialize();

            engine.Run();
        }
    }
}
