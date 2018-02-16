using System;
using System.Collections.Generic;
using System.Linq;

namespace CardSharp.Rules
{
    public class RuleChain2 : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cardGroups, List<CardGroup> lastCardGroups)
        {
            var first = cardGroups.First();
            if (lastCardGroups != null)
            {
                if (cardGroups.Count != lastCardGroups.Count) // 与之前张数必须相同
                    return false;
                if (first.Amount <= lastCardGroups.First().Amount) // 必须比前面的大
                    return false;
            }

            for (var index = first.Amount; index < cardGroups.Count + first.Amount; index++)
            {
                var cardGroup = cardGroups[index - first.Amount];
                if (cardGroup.Count != 2) // 必须只有2张
                    return false;
                if (index != cardGroup.Amount) // 必须连起来
                    return false;
                if (index == Constants.Cards.C2) // 不能是2
                    return false;
            }

            if (cardGroups.Count < 6 / 2)
                return false; // 必须大于6张

            return true;
        }

        public override string ToString()
        {
            return "双顺";
        }

        public override (bool exists, List<Card> cards) FirstMatchedCards(List<CardGroup> sourceGroups, List<CardGroup> lastCardGroups)
        {
            return sourceGroups.ExtractChain(lastCardGroups?.Count ?? 3, 2, lastCardGroups?.First().Amount ?? -1);
        }
    }
}