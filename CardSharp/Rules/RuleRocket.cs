using System.Collections.Generic;
using System.Linq;

namespace CardSharp.Rules
{
    public class RuleRocket : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cardGroups, List<CardGroup> lastCardGroups)
        {
            if (cardGroups.Count != 2) return false;

            if (cardGroups.First().Amount != Constants.Cards.CGhost) return false;

            if (cardGroups.Last().Amount != Constants.Cards.CKing) return false;

            return true;
        }

        public override (bool exists, List<Card> cards) FirstMatchedCards(List<CardGroup> sourceGroups,
            List<CardGroup> lastCardGroups)
        {
            var gr = sourceGroups.Any(g => g.Amount == Constants.Cards.CGhost) &&
                     sourceGroups.Any(g => g.Amount == Constants.Cards.CKing);
            if (gr)
            {
                return (true, new List<Card>
                {
                    new Card(Constants.Cards.CGhost),
                    new Card(Constants.Cards.CKing)
                });
            }
            else
            {
                return default;
            }
        }
    }
}