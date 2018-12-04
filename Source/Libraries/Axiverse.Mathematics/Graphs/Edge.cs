using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Graphs
{
    public class Edge<T>
    {
        public Node<T> Former { get; set; }
        public Node<T> Latter { get; set; }
    }
}
