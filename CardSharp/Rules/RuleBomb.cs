using System.Collections.Generic;
using System.Linq;

namespace CardSharp.Rules
{
    /// <summary>
    ///     3333
    /// </summary>
    public class RuleBomb : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cards, List<CardGroup> lastCards)
        {
            // ensure single group
            if (cards.Count != 1)
                return false;
            if (lastCards != null && lastCards.Count == 1 && lastCards.First().Count == 4)
                return SingleGroupMatch.IsMatch(cards, lastCards, 4);
            return true;
        }

        public override string ToString(List<CardGroup> cards)
        {
            return "炸弹";
        }
    }
}