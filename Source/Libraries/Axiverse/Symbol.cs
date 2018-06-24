using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse
{
    /// <summary>
    /// A monotonic symbol.
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// Gets the unique identifier for the symbol.
        /// </summary>
        public Guid Identifier { get; }

        /// <summary>
        /// Gets the description for the symbol.
        /// </summary>
        public string Description { get; }

        private Symbol(Guid guid, string description)
        {
            Identifier = Identifier;
            Description = Description;
        }

        /// <summary>
        /// Returns the unique symbol given the specified identifier and description.
        /// </summary>
        /// <param name="guid">The identifier of the symbol.</param>
        /// <param name="description">The description of the symbol.</param>
        /// <returns>The unique symbol with the specified identifier and description.</returns>
        /// <exception cref="Exception">Throws when the description doesn't match the description of the existing symbol.</exception>
        public static Symbol Of(Guid guid, string description)
        {
            if (symbols.TryGetValue(guid, out var symbol))
            {
                Requires.That(symbol.Description == description);
            }
            else
            {
                symbol = new Symbol(guid, description);
                symbols.Add(guid, symbol);
            }
            return symbol;
        }

        private static readonly Dictionary<Guid, Symbol> symbols = new Dictionary<Guid, Symbol>();

        /// <summary>
        /// Gets the symbol with the empty identifier.
        /// </summary>
        public static readonly Symbol Empty = Symbol.Of(Guid.Empty, "Symbol.Empty");
    }
}
