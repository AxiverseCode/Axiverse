using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Symbolics
{
    public class Text : Symbol
    {
        public string Value { get; }

        public Text(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("\"{0}\"", Value);
        }
    }
}
