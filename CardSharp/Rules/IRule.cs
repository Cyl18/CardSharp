using System.Collections.Generic;

namespace CardSharp.Rules
{
    public interface IRule
    {
        bool IsMatch(List<CardGroup> cardGroups, List<CardGroup> lastCardGroups);
        string ToString();
        (bool exists, List<Card> cards) FirstMatchedCards(List<CardGroup> sourceGroups, List<CardGroup> lastCardGroups);
    }
}