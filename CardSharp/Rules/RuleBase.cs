using System.Collections.Generic;

namespace CardSharp.Rules
{
    public abstract class RuleBase : IRule
    {
        public string CurrentError { get; protected set; }
        public abstract bool IsMatch(List<CardGroup> cardGroups, List<CardGroup> lastCardGroups);
        public abstract string ToString();
        public abstract (bool exists, List<Card> cards) FirstMatchedCards(List<CardGroup> sourceGroups, List<CardGroup> lastCardGroups);
    }
}