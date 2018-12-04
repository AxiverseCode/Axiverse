using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Graphs
{
    public class Node<T>
    {
        List<T> Outgoing { get; } = new List<T>();
        List<T> Incoming { get; } = new List<T>();
    }
}
