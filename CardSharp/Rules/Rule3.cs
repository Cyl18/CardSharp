using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp.Rules
{
    public class Rule3 : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cards, List<CardGroup> lastCards)
        {
            if (cards.Count != 1) return false;
            var first = cards.First();
            return first.Count == 3 && (lastCards == null || lastCards.First().Amount < first.Amount);
        }

        public override string ToString(List<CardGroup> cards)
        {
            return "3带0";
        }
    }
}
