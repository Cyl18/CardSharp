using System.Collections.Generic;
using System.Linq;

namespace CardSharp.Rules
{
    public class Rule3 : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cardGroups, List<CardGroup> lastCardGroups)
        {
            return SingleGroupMatch.IsMatch(cardGroups, lastCardGroups, 3);
        }

        public override string ToString()
        {
            return "3带0";
        }

        public override (bool exists, List<Card> cards) FirstMatchedCards(List<CardGroup> sourceGroups,
            List<CardGroup> lastCardGroups)
        {
            var bombs = sourceGroups.Where(group => group.Count >= 3).ToList();
            if (bombs.Count == 0)
                return (false, null); //没有炸弹
            if (lastCardGroups == null) return (exists: true, cards: bombs.First().ToEnumerable().ToCards().ToList());

            var sbombs = bombs.Where(bomb => bomb.Amount > lastCardGroups.First().Amount).ToList();
            if (sbombs.Count == 0)
                return (false, null); //没有大于的炸弹
            return (exists: true, cards: sbombs.First().ToEnumerable().ToCards().ToList());
        }
    }
}