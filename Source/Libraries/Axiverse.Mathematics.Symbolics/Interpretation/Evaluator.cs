using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Symbolics.Interpretation
{
    public class Evaluator
    {
        protected delegate Symbol Transformation(Expression expression, Context context);

        public delegate Symbol Functor(Context context);

        public delegate Symbol Functor<T>(T t, Context context)
            where T : Symbol;

        public delegate Symbol Functor<T, U>(T t, U u, Context context)
            where T : Symbol
            where U : Symbol;

        public delegate Symbol Functor<T, U, V>(T t, U u, V v, Context context)
            where T : Symbol
            where U : Symbol
            where V : Symbol;

        protected Transformation Transform { get; }

        protected Evaluator(Transformation transform)
        {
            Transform = transform;
        }

        public Symbol Evaluate(Symbol symbol, Context context)
        {
            return Transform((Expression)symbol, context);
        }

        public static Evaluator FromLambda(Functor functor)
        {
            return new Evaluator((e, c) =>
            {
                Requires.That(e.Count == 1);

                return functor(c);
            });
        }

        public static Evaluator FromLambda<T>(Functor<T> functor)
            where T : Symbol
        {
            return new Evaluator((e, c) =>
            {
                Requires.That(e.Count == 2);

                var t = (T)e[1];

                return functor(t, c);
            });
        }

        public static Evaluator FromLambda<T, U>(Functor<T, U> functor)
            where T : Symbol
            where U : Symbol
        {
            return new Evaluator((e, c) =>
            {
                Requires.That(e.Count == 3);

                var t = (T)e[1];
                var u = (U)e[2];

                return functor(t, u, c);
            });
        }

        public static Evaluator FromLambda<T, U, V>(Functor<T, U, V> functor)
            where T : Symbol
            where U : Symbol
            where V : Symbol
        {
            return new Evaluator((e, c) =>
            {
                Requires.That(e.Count == 4);

                var t = (T)e[1];
                var u = (U)e[2];
                var v = (V)e[3];

                return functor(t, u, v, c);
            });
        }
    }
}
