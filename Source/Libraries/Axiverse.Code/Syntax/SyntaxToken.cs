using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Code.Syntax
{
    public class SyntaxToken : SyntaxNode
    {
        public String Value { get; set; }

        public SyntaxToken()
        {
            Children = null;
        }

        public override string ToString()
        {
            return $"Token {Type} : {Value}";
        }
    }
}
