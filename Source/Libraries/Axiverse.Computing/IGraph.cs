using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Computing
{
    public interface IGraph : IGraph<string>
    {
        string Name { set; }
        float Delta { set; }
    }

    public interface IGraph<T>
    {
        Task<T> Start();
    }
}
