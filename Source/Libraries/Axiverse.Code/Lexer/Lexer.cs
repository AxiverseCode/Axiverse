using Axiverse.Code.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Code.Lexer
{
    public class Lexer
    {
        public Dictionary<String, State> States { get; } = new Dictionary<string, State>();

        public Stack<State> StateStack { get; } = new Stack<State>();

        public SyntaxNode Parse(String value)
        {
            var root = new SyntaxNode
            {
                Type = "CompilationUnit"
            };

            var startat = 0;
            var current = root;

            next:
            while (startat < value.Length)
            {
                var state = StateStack.Peek();
                
                foreach (var rule in state.Rules)
                {
                    var match = rule.Matcher.Match(value, startat);
                    Console.WriteLine($"Matching @{startat} to {rule.Classification}");

                    if (match.Success && match.Length > 0)
                    {
                        switch (rule.Type)
                        {
                            case RuleType.Token:
                                var token = new SyntaxToken()
                                {
                                    Parent = current,
                                    Type = rule.Classification,
                                    Value = match.ToString(),
                                };
                                current.Children.Add(token);
                                break;
                            case RuleType.PushState:
                                var node = new SyntaxNode()
                                {
                                    Parent = current,
                                    Type = rule.Classification,
                                };
                                current.Children.Add(node);
                                StateStack.Push(States[rule.Transition]);
                                current = node;
                                break;
                            case RuleType.PopState:
                                StateStack.Pop();
                                current = current.Parent;
                                break;
                            case RuleType.Ignore:
                            default:
                                break;
                        }
                        startat += match.Length;
                        Console.WriteLine($"Matched \"{ match }\" to {rule.Classification}");
                        goto next;
                    }
                }
                throw new ArgumentException();
            }

            return root;
        }


    }
}
