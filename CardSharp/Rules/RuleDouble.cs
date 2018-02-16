using System.Collections.Generic;

namespace CardSharp.Rules
{
    /// <summary>
    ///     33
    /// </summary>
    public class RuleDouble : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cards, List<CardGroup> lastCards)
        {
            return SingleGroupMatch.IsMatch(cards, lastCards, 2);
        }

        public override string ToString()
        {
            return "对子";
        }
    }
}