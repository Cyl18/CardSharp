using System;
using System.Collections.Generic;
using System.Linq;

namespace CardSharp.Rules
{
    public class RuleAirplain2 : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cardGroups, List<CardGroup> lastCardGroups)
        {
            var first = cardGroups.FirstOrDefault(card => card.Count == 3);
            if (first == null) return false;
            
            if (lastCardGroups != null)
            {
                if (cardGroups.Count != lastCardGroups.Count) // 与之前张数必须相同
                    return false;
                if (first.Amount <= lastCardGroups.First(card => card.Count == 3).Amount) // 必须比前面的大
                    return false;
            }

            var count = cardGroups.Count(card => card.Count == 3);
            for (var index = first.Amount; index < count + first.Amount; index++)
            {
                var cardGroup = cardGroups[index - first.Amount];
                if (cardGroup.Count != 3) // 必须只有3张
                    return false;
                if (index != cardGroup.Amount) // 必须连起来
                    return false;
                if (index == Constants.Cards.C2) // 不能是2
                    return false;
            }

            if (cardGroups.Count <= 4)
                return false; // 必须大于4组

            if (Math.Abs(cardGroups.Count / 2.0 - count) > 0.1)
                return false; // 单张的张数必须与多张的相同

            if (!cardGroups.Where(card => card.Count != 3).ToList().TrueForAll(card => card.Count == 2))
                return false; // 单张的张数必须为2

            return true;
        }

        public override string ToString()
        {
            return "飞机带大翅膀";
        }

        public override (bool exists, List<Card> cards) FirstMatchedCards(List<CardGroup> sourceGroups, List<CardGroup> lastCardGroups)
        {
            throw new NotImplementedException();
        }
    }
}