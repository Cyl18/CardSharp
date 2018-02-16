using System.Collections.Generic;
using System.Linq;

namespace CardSharp.Rules
{
    public class Rule3With1 : RuleBase
    {
        // ATTENTION: lastCards可能为null 如果传进来绝对是当前的规则
        public override bool IsMatch(List<CardGroup> cards, List<CardGroup> lastCards)
        {
            if (cards.Count != 2) // 必须两组
                return false;
            if (!(cards.Any(card => card.Count == 3) && cards.Any(card => card.Count == 1))) // 匹配牌形
                return false;
            return lastCards == null || cards.First(card => card.Count == 3).Amount >
                   lastCards.First(card => card.Count == 3).Amount; // 匹配大小
        }

        public override string ToString()
        {
            return "3带1";
        }
    }
}