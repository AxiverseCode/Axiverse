using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Symbolics.Interpretation
{
    public class Evaluator
    {
        public delegate bool Transformation(Expression expression, Context context, out Symbol symbol);

        public delegate Symbol Functor(Context context);

        public delegate Symbol CollectionFunctor<T>(T[] t, Context context)
            where T : Symbol;

        public delegate Symbol Functor<T>(T t, Context context)
            where T : Symbol;

        public delegate Symbol Functor<T, U>(T t, U u, Context context)
            where T : Symbol
            where U : Symbol;

        public delegate Symbol Functor<T, U, V>(T t, U u, V v, Context context)
            where T : Symbol
            where U : Symbol
            where V : Symbol;

        public List<Transformation> Transforms { get; } = new List<Transformation>();

        protected Evaluator(Transformation transform)
        {
            Transforms.Add(transform);
        }

        public Symbol Evaluate(Symbol symbol, Context context)
        {
            bool evaluated = false;
            foreach (var transform in Transforms)
            {
                if (transform((Expression)symbol, context, out var result))
                {
                    if (evaluated)
                    {
                        return Atom.OfName("Ambiguous");
                    }

                    symbol = result;
                    evaluated = true;
                }
            }
            return symbol;
        }

        public static Evaluator FromLambda<T>(CollectionFunctor<T> functor)
            where T : Symbol
        {
            return new Evaluator((Expression e, Context c, out Symbol s) =>
            {
                s = null;
                if (e.Count == 0)
                {
                    return false;
                }

                var arguments = new T[e.Count - 1];

                for (int i = 0; i < e.Count - 1; i++)
                {
                    if (e[i + 1] is T t)
                    {
                        arguments[i] = t;
                    }
                    else
                    {
                        return false;
                    }
                }

                s = functor(arguments, c);
                return true;
            });
        }

        public static Evaluator FromLambda(Functor functor)
        {
            return new Evaluator((Expression e, Context c, out Symbol s) =>
            {
                s = null;
                if (e.Count < 1)
                {
                    return false;
                }

                s = functor(c);
                return true;
            });
        }

        public static Evaluator FromLambda<T>(Functor<T> functor)
            where T : Symbol
        {
            return new Evaluator((Expression e, Context c, out Symbol s) =>
            {
                s = null;
                if (e.Count < 2)
                {
                    return false;
                }

                if (!(e[1] is T t))
                {
                    return false;
                }

                s = functor(t, c);
                return true;
            });
        }

        public static Evaluator FromLambda<T, U>(Functor<T, U> functor)
            where T : Symbol
            where U : Symbol
        {
            return new Evaluator((Expression e, Context c, out Symbol s) =>
            {
                s = null;
                if (e.Count < 3)
                {
                    return false;
                }

                if (!(e[1] is T t))
                {
                    return false;
                }
                if (!(e[2] is U u))
                {
                    return false;
                }

                s = functor(t, u, c);
                return true;
            });
        }

        public static Evaluator FromLambda<T, U, V>(Functor<T, U, V> functor)
            where T : Symbol
            where U : Symbol
            where V : Symbol
        {
            return new Evaluator((Expression e, Context c, out Symbol s) =>
            {
                s = null;
                if (e.Count < 4)
                {
                    return false;
                }

                if (!(e[1] is T t))
                {
                    return false;
                }
                if (!(e[2] is U u))
                {
                    return false;
                }
                if (!(e[3] is V v))
                {
                    return false;
                }


                s = functor(t, u, v, c);
                return true;
            });
        }
    }
}
