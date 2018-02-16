using System.Collections.Generic;
using System.Linq;

namespace CardSharp.Rules
{
    public class Rule4With4 : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cards, List<CardGroup> lastCards)
        {
            if (cards.Count != 3) // 必须3组
                return false;
            if (!(cards.Any(card => card.Count == 4) && cards.Any(card => card.Count == 2))) // 匹配牌形
                return false;
            return lastCards == null || cards.First(card => card.Count == 4).Amount >
                   lastCards.First(card => card.Count == 4).Amount; // 匹配大小
        }

        public override string ToString()
        {
            return "4带2对";
        }
    }
}