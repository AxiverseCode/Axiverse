using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Symbolics
{
    public class Atom : Symbol
    {
        public string Name { get; }

        public Atom(string name) : base(0)
        {
            Name = name;
        }

        public static Atom OfName(string name)
        {
            if (named.TryGetValue(name, out var value))
            {
                return value;
            }
            value = new Atom(name);
            named.Add(name, value);
            return value;
        }

        public static Atom OfHistory(int index)
        {
            return OfName(string.Format("${0}", index));
        }

        public override string ToString()
        {
            return Name;
        }

        private static Dictionary<string, Atom> named = new Dictionary<string, Atom>();
    }
}
