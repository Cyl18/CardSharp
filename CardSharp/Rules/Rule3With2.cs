using System.Collections.Generic;
using System.Linq;

namespace CardSharp.Rules
{
    public class Rule3With2 : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cardGroups, List<CardGroup> lastCardGroups)
        {
            if (cardGroups.Count != 2) // 必须两组
                return false;
            if (!(cardGroups.Any(card => card.Count == 3) && cardGroups.Any(card => card.Count == 2))) // 匹配牌形
                return false;
            return lastCardGroups == null || cardGroups.First(card => card.Count == 3).Amount >
                   lastCardGroups.First(card => card.Count == 3).Amount; // 匹配大小
        }

        public override (bool exists, List<Card> cards) FirstMatchedCards(List<CardGroup> sourceGroups,
            List<CardGroup> lastCardGroups)
        {
            var c3s = sourceGroups.Where(group => group.Count == 3).ToList();
            var coth = sourceGroups.Where(group => group.Count != 3 && group.Count > 1).ToList();
            if (c3s.Count == 0 || coth.Count == 0)
                return (false, null);
            if (lastCardGroups == null) return (true, ToList(c3s, coth));

            {
                var sc3s = c3s.Where(group => group.Amount > lastCardGroups.First(g => g.Count == 3).Amount).ToList();
                if (sc3s.Count == 0)
                    return (false, null);
                return (true, ToList(sc3s, coth));
            }

            List<Card> ToList(List<CardGroup> sc3s, List<CardGroup> cardGroups)
            {
                return sc3s.First().ToEnumerable().ToCards()
                    .Concat(new CardGroup(cardGroups.First().Amount, 2).ToEnumerable().ToCards()).ToList();
            }
        }
    }
}