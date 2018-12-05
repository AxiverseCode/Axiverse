using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Symbolics.Interpretation
{
    public class Interpreter
    {
        public Context Context { get; set; }
        public List<Symbol> Inputs { get; set; } = new List<Symbol>();
        public bool Running { get; set; }

        public Interpreter()
        {
            Context = new Context();
            Context.Evaluators[Atom.OfName("Exit")] = Evaluator.FromLambda((c) =>
            {
                Running = false;
                return Expression.Empty;
            });

            Context.Evaluators[Atom.OfName("+")] = Evaluator.FromLambda(
                (Numeric[] n, Context c) => new Numeric(n.Sum(i => i.Value)));
            Context.Evaluators[Atom.OfName("-")] = Evaluator.FromLambda(
                (Numeric u, Numeric v, Context c) => new Numeric(u.Value - v.Value));
            Context.Evaluators[Atom.OfName("*")] = Evaluator.FromLambda(
                (Numeric[] n, Context c) => n.Aggregate((i, j) => new Numeric(i.Value * j.Value)));
            Context.Evaluators[Atom.OfName("/")] = Evaluator.FromLambda(
                (Numeric u, Numeric v, Context c) => new Numeric(u.Value / v.Value));
        }

        public void Run()
        {
            Running = true;
            while (Running)
            {
                // var key = Console.ReadKey();
                var entry = Context.Parse(Console.ReadLine());
                Inputs.Insert(0, entry);

                var output = Context.Evaluate(entry);
                Console.WriteLine(output.ToString());
            }
        }
        // 1. Read
        // 2. Parse
        // 3. Execute
        // 4. Loop
    }
}
