using System.Collections.Generic;

namespace CardSharp.Rules
{
    public interface IRule
    {
        bool IsMatch(List<CardGroup> cards, List<CardGroup> lastCards);
        string ToString(List<CardGroup> cards);
    }
}