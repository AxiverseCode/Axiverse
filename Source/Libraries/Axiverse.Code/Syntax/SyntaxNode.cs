using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Code.Syntax
{
    public class SyntaxNode
    {
        public SyntaxNode Parent { get; set; }

        public List<SyntaxNode> Children { get; protected set; }

        public String Type { get; set; }

        public SyntaxNode()
        {
            Children = new List<SyntaxNode>();
        }

        public override string ToString()
        {
            return $"Node {Type}, children {Children.Count}";
        }
    }
}
