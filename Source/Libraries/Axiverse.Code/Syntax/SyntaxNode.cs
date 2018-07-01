using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Code.Syntax
{
    public class SyntaxNode
    {
        private SyntaxNode parent;
        internal SyntaxTree syntaxTree;

        public int Position { get; set;  }

        public int Length { get; set; }

        public TextSpan TextSpan
        {
            get
            {
                return new TextSpan(Position, Length);
            }
        }


        // Child (they are either a node or token)
    }
}
