using System.Collections.Generic;
using System.Linq;

namespace CardSharp.Rules
{
    public class RuleRocket : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cardGroups, List<CardGroup> lastCardGroups)
        {
            return cardGroups.Count == 2 &&
                   cardGroups.First().Amount == Constants.Cards.CGhost &&
                   cardGroups.Last().Amount == Constants.Cards.CKing;
        }

        public override string ToString()
        {
            return "火箭";
        }

        public override (bool exists, List<Card> cards) FirstMatchedCards(List<CardGroup> sourceGroups, List<CardGroup> lastCardGroups)
        {
            return (false, null);
        }
    }
}