using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp.Rules
{
    public class RuleAirplain2 : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cards, List<CardGroup> lastCards)
        {
            var first = cards.First(card => card.Count == 3);
            if (lastCards != null) {
                if (cards.Count != lastCards.Count) // 与之前张数必须相同
                    return false;
                if (first.Amount <= lastCards.First(card => card.Count == 3).Amount) // 必须比前面的大
                    return false;
            }

            var count = cards.Count(card => card.Count == 3);
            for (var index = first.Amount; index < count + first.Amount; index++) {
                var cardGroup = cards[index- first.Amount];
                if (cardGroup.Count != 3) // 必须只有3张
                    return false;
                if (index != cardGroup.Amount) // 必须连起来
                    return false;
                if (index == Constants.Cards.C2) // 不能是2
                    return false;
            }

            if (cards.Count < 4)
                return false; // 必须大于4组

            if (Math.Abs(cards.Count / 2.0 - count) > 0.1)
                return false; // 单张的张数必须与多张的相同

            if (!cards.Where(card => card.Count != 3).ToList().TrueForAll(card => card.Count == 2))
                return false; // 单张的张数必须为2

            return true;
        }

        public override string ToString(List<CardGroup> cards)
        {
            return "飞机带大翅膀";
        }
    }
}
