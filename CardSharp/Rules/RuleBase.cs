using System.Collections.Generic;

namespace CardSharp.Rules
{
    public abstract class RuleBase : IRule
    {
        public abstract bool IsMatch(List<CardGroup> cards, List<CardGroup> lastCards);
        public abstract string ToString(List<CardGroup> cards);
    }
}