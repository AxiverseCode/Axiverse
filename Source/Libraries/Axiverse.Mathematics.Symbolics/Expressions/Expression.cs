using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Symbolics
{
    public class Expression : Symbol
    {
        public Expression(params Symbol[] symbols) : base(symbols) { }

        public override string ToString()
        {
            if (Count == 0)
            {
                return "( )";
            }

            StringBuilder builder = new StringBuilder("( ");

            foreach (var symbol in this)
            {
                builder.Append(symbol);
                builder.Append(", ");
            }

            builder.Remove(builder.Length - 2, 2);
            builder.Append(" )");

            return builder.ToString();
        }

        public static readonly Expression Empty = new Expression();

        public class Builder : Symbol
        {
            public List<Symbol> Arguments { get; } = new List<Symbol>();

            public override int Count => Arguments.Count;

            public override Symbol this[int index] => Arguments[index];

            public Builder()
            {

            }

            public Builder(Expression expression)
            {
                Arguments.AddRange(expression);
            }

            public Expression Build()
            {
                Symbol[] symbols = new Symbol[Count];

                for (int i = 0; i < Arguments.Count; i++)
                {
                    if (this[i] is Builder builder)
                    {
                        symbols[i] = builder.Build();
                    }
                    else
                    {
                        symbols[i] = this[i];
                    }
                }

                return new Expression(symbols);
            }
        }
    }
}
