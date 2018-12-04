using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Mathematics.Symbolics
{
    public abstract class Symbol : IEnumerable<Symbol>
    {
        private readonly Symbol[] arguments;

        /// <summary>
        /// Gets the count of children of this symbol.
        /// </summary>
        public virtual int Count => arguments.Length;

        /// <summary>
        /// Gets or sets a child of this symbol.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual Symbol this[int index]
        {
            get => arguments[index];
        }

        protected Symbol(int count)
        {
            this.arguments = new Symbol[count];
        }

        protected Symbol(params Symbol[] arguments)
        {
            this.arguments = arguments;
        }

        public IEnumerator<Symbol> GetEnumerator()
        {
            return ((IEnumerable<Symbol>)arguments).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
