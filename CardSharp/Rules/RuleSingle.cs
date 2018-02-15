using System.Collections.Generic;

namespace CardSharp.Rules
{
    /// <summary>
    ///     3
    /// </summary>
    public class RuleSingle : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cards, List<CardGroup> lastCards)
        {
            return SingleGroupMatch.IsMatch(cards, lastCards, 1);
        }

        public override string ToString(List<CardGroup> cards)
        {
            return "单张";
        }
    }
}