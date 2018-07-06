using Axiverse.Code.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HelloCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var whitespaceRule = new Rule(@"[\s]+", RuleType.Ignore, "whitespace");
            var openRule = new Rule(@"\{", RuleType.PushState, "open", "standard");
            var closeRule = new Rule(@"\}", RuleType.PopState, "close");
            var tokenRule = new Rule("[A-z0-9]+", RuleType.Token, "token");

            var standardState = new State("standard", whitespaceRule, openRule, closeRule, tokenRule);

            var lexer = new Lexer();
            lexer.States.Add(standardState.Name, standardState);
            lexer.StateStack.Push(standardState);

            var code =
                @"hello { a b c { d e f } { h i j } 
                }";

            var tree = lexer.Parse(code);

            Console.Read();
        }
    }
}
