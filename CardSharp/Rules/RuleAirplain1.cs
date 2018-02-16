using System;
using System.Collections.Generic;
using System.Linq;

namespace CardSharp.Rules
{
    public class RuleAirplain1 : RuleBase
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
                return false; // 必须大于等于4组

            if (Math.Abs(cardGroups.Count / 2.0 - count) > 0.1)
                return false; // 单张的张数必须与多张的相同

            if (!cardGroups.Where(card => card.Count != 3).ToList().TrueForAll(card => card.Count == 1))
                return false; // 单张的张数必须为1

            return true;
        }

        public override string ToString()
        {
            return "小翼飞机";
        }

        public override (bool exists, List<Card> cards) FirstMatchedCards(List<CardGroup> sourceGroups, List<CardGroup> lastCardGroups)
        {
            if (lastCardGroups != null)
            {
                var last = lastCardGroups.ExtractChain(3, 2, -1).result;
                var min = last.First().Amount;
                var c3s = last.Count;
                var chain = sourceGroups.ExtractChain(c3s, 3, min);
                if (!chain.exists) return default;
                var nonchains = sourceGroups.IsTargetVaildAndRemove(chain.result.ExtractCardGroups()).result.ExtractCardGroups();
                if (nonchains.Count < c3s) return default;
                var c3 = nonchains.Take(c3s).ToCards();
                return (true, chain.result.Concat(c3).ToList());
            }
            else
            {
                var chain = sourceGroups.ExtractChain(3, 2, -1);
                if (!chain.exists) return default;
                var nonchains = sourceGroups.IsTargetVaildAndRemove(chain.result.ExtractCardGroups()).result.ExtractCardGroups();
                var c3s = 2;
                if (nonchains.Count < c3s)
                    return default;
                var c3 = nonchains.Take(c3s).ToCards();
                return (true, chain.result.Concat(c3).ToList());
            }
        }
    }
}