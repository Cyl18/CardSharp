using System.Collections.Generic;
using System.Linq;

namespace CardSharp.Rules
{
    /// <summary>
    ///     3333
    /// </summary>
    public class RuleBomb : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cardGroups, List<CardGroup> lastCardGroups)
        {
            // ensure single group
            if (cardGroups.Count != 1 || cardGroups.First().Count != 4)
                return false;
            if (lastCardGroups != null && lastCardGroups.Count == 1 && lastCardGroups.First().Count == 4)
                return SingleGroupMatch.IsMatch(cardGroups, lastCardGroups, 4);
            return true;
        }

        public override string ToString()
        {
            return "炸弹";
        }

        public override (bool exists, List<Card> cards) FirstMatchedCards(List<CardGroup> sourceGroups, List<CardGroup> lastCardGroups)
        {
            var bombs = sourceGroups.Where(group => group.Count >= 4).ToList();
            if (bombs.Count == 0)
                return (false, null); //没有炸弹
            if (lastCardGroups == null) {
                return (exists: true, cards: bombs.First().ToEnumerable().ToCards().ToList());
            } else {
                var sbombs = bombs.Where(bomb => bomb.Amount > lastCardGroups.First().Amount).ToList();
                if (sbombs.Count == 0)
                    return (false, null); //没有大于的炸弹
                return (exists: true, cards: sbombs.First().ToEnumerable().ToCards().ToList());
            }
        }
    }
}