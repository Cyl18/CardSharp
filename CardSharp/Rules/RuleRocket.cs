using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp.Rules
{
    public class RuleRocket : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cards, List<CardGroup> lastCards)
        {
            return cards.Count == 2 && 
                   cards.First().Amount == Constants.Cards.CGhost &&
                   cards.Last().Amount == Constants.Cards.CKing;
        }

        public override string ToString(List<CardGroup> cards)
        {
            return "火箭";
        }
    }
}
