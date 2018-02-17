using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp.Rules
{
    public static class Chain
    {
        public static (bool exists, List<Card> result) ExtractChain(this List<CardGroup> from, int chainNum,/*最少要连多少次*/ int singleChainCardsNum,/*一个连要多少张牌*/int min/*最小的牌*/)
        {
            var matches = -1; // 满足的牌量
            var cardIndex = -1; // 累加: 当前应该是的牌
            var firstCard = -1; // 第一张对子的面值
            var lastCard = -1;
            foreach (var cardGroup in from.Where(cg => cg.Amount > min && cg.Amount != Constants.Cards.C2)) {
                if (cardGroup.Count < singleChainCardsNum)
                {
                    matches = 0;
                    cardIndex = -1;
                    continue;
                }

                var currentAmount = cardGroup.Amount; // 当前面值
                if (cardIndex == currentAmount) {
                    matches++;
                    lastCard = currentAmount;
                    cardIndex++;
                } else {
                    matches = 1;
                    firstCard = currentAmount;
                    cardIndex = currentAmount + 1;
                    continue;
                }


                if (chainNum == matches) {
                    // 成功了.
                    var o = new List<Card>();
                    var amounts = Enumerable.Range(firstCard, matches);
                    foreach (var amount in amounts) {
                        for (int i = 0; i < singleChainCardsNum; i++) {
                            o.Add(new Card(amount));
                        }
                    }

                    return (true, o);
                }
            }

            return (false, null);
        }
    }
}
