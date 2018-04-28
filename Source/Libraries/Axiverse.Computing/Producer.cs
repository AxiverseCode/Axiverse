using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Computing
{
    public class Producer
    {
        public void Bind(Type graph)
        {
            Contract.Requires(graph.IsInterface);
            Contract.Requires(typeof(IGraph).IsAssignableFrom(graph));
        }
    }
}
