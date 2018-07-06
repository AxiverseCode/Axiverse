using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Code.Lexer
{
    public class State
    {
        public String Name { get; set; }

        public List<Rule> Rules { get; } = new List<Rule>();

        public State(String name, params Rule[] rules)
        {
            Name = name;
            Rules.AddRange(rules);
        }
    }
}
