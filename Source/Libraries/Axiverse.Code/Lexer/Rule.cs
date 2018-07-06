using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Axiverse.Code.Lexer
{
    public class Rule
    {
        /// <summary>
        /// Gets the initial match regex matcher.
        /// </summary>
        public Regex Matcher { get; set; }

        /// <summary>
        /// Gets or sets they type of the rule.
        /// </summary>
        public RuleType Type { get; set; }

        /// <summary>
        /// Gets or sets the target. This is the token type for Token rules or next State for state transitions.
        /// </summary>
        public String Classification { get; set; }

        public String Transition { get; set; }

        public Rule(String pattern, RuleType type, String classification)
        {
            if (!pattern.StartsWith("^"))
            {
                pattern = @"\G" + pattern;
            }

            Matcher = new Regex(pattern, RegexOptions.Singleline);
            Type = type;
            Classification = classification;
        }

        public Rule(String pattern, RuleType type, String classification, String transition)
            : this(pattern, type, classification)
        {
            Transition = transition;
        }

    }

    public enum RuleType
    {
        Ignore,
        Token,
        PushState,
        PopState,
    }
}
