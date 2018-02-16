using System.Collections.Generic;
using System.Linq;

namespace CardSharp.Rules
{
    public class Rule3With1 : RuleBase
    {
        // ATTENTION: lastCards可能为null 如果传进来绝对是当前的规则
        public override bool IsMatch(List<CardGroup> cardGroups, List<CardGroup> lastCardGroups)
        {
            if (cardGroups.Count != 2) // 必须两组
                return false;
            if (!(cardGroups.Any(card => card.Count == 3) && cardGroups.Any(card => card.Count == 1))) // 匹配牌形
                return false;
            return lastCardGroups == null || cardGroups.First(card => card.Count == 3).Amount >
                   lastCardGroups.First(card => card.Count == 3).Amount; // 匹配大小
        }

        public override string ToString()
        {
            return "3带1";
        }

        public override (bool exists, List<Card> cards) FirstMatchedCards(List<CardGroup> sourceGroups, List<CardGroup> lastCardGroups)
        {
            var c3s = sourceGroups.Where(group => group.Amount == 3).ToList();
            var coth = sourceGroups.Where(group => group.Amount != 3).ToList();
            if (c3s.Count == 0 || coth.Count == 0) return (false, null);
            if (lastCardGroups==null)
            {
                return (true, ToList(c3s, coth));
            }
            else
            {
                var sc3s = c3s.Where(group => group.Amount > lastCardGroups.First(g => g.Count == 3).Amount).ToList();
                if (sc3s.Count == 0) return (false, null);
                return (true, ToList(sc3s, coth));
            }

            List<Card> ToList(List<CardGroup> sc3s, List<CardGroup> cardGroups)
            {
                return sc3s.First().ToEnumerable().ToCards().Concat(new CardGroup(cardGroups.First().Amount, 1).ToEnumerable().ToCards()).ToList();
            }
        }

        
    }
}