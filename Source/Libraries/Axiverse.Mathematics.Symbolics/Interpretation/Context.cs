using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Symbolics.Interpretation
{
    public class Context
    {
        public Dictionary<Symbol, Evaluator> Evaluators { get; } = new Dictionary<Symbol, Evaluator>();
        public Dictionary<Symbol, Symbol> Variable { get; } = new Dictionary<Symbol, Symbol>();
        public List<Symbol> History { get; } = new List<Symbol>();

        public Symbol Evaluate(Symbol symbol)
        {
            if (symbol is Expression expression && expression.Count > 0)
            {
                Expression.Builder builder = new Expression.Builder(expression);
                for (int i = 0; i < builder.Count; i++)
                {
                    if (builder.Arguments[i] is Expression)
                    {
                        builder.Arguments[i] = Evaluate(builder[i]);
                    }
                }

                if (Evaluators.TryGetValue(builder[0], out var evaluator))
                {
                    symbol = evaluator.Evaluate(builder.Build(), this);
                }
            }

            History.Insert(0, symbol);

            for (int i = 0; i < Math.Min(History.Count, 10); i++)
            {
                Variable[Atom.OfHistory(i)] = History[i];
            }

            return symbol;
        }

        public static Symbol Parse(string value)
        {
            Stack<Expression.Builder> groupings = new Stack<Expression.Builder>();
            var root = new Expression.Builder();
            groupings.Push(root);

            for (int i = 0; i < value.Length;)
            {
                switch (value[i])
                {
                    case '(':
                        var group = new Expression.Builder();
                        groupings.Peek().Arguments.Add(group);
                        groupings.Push(group);

                        i++;
                        break;
                    case ')':
                        groupings.Pop();

                        i++;
                        break;
                    default:
                        if (value[i] == ',' || char.IsWhiteSpace(value[i]))
                        {
                            i++;
                        }
                        else if (value[i] == '"')
                        {
                            var end = value.IndexOfAny(new char[] { '"' }, ++i);
                            var text = value.Substring(i, end - i);

                            groupings.Peek().Arguments.Add(new Text(text));

                            i = end + 1;
                        }
                        else if (char.IsNumber(value[i]))
                        {
                            var end = value.IndexOfAny(new char[] { ' ', ',', ')' }, i);

                            if (end == -1)
                            {
                                end = value.Length;
                            }

                            var number = value.Substring(i, end - i);

                            groupings.Peek().Arguments.Add(new Numeric(double.Parse(number)));

                            i = end;
                        }
                        else
                        {
                            var end = value.IndexOfAny(new char[] { ' ', ',', ')' }, i);

                            if (end == -1)
                            {
                                end = value.Length;
                            }

                            var text = value.Substring(i, end - i);

                            groupings.Peek().Arguments.Add(Atom.OfName(text));

                            i = end;
                        }
                        break;
                }
            }

            if (root.Arguments.Count > 1)
            {
                return root.Build();
            }
            else if (root.Arguments.Count == 1)
            {
                return root.Build()[0];
            }
            return Expression.Empty;
        }
    }
}
