using System.Collections.Generic;
using System.Linq;

namespace CardSharp.Rules
{
    public class SingleGroupMatch
    {
        public static bool IsMatch(List<CardGroup> cards, List<CardGroup> lastCards, int count)
        {
            var card1 = cards.First();

            if (lastCards == null) return cards.Count == 1 && card1.Count == count; // 单张

            if (cards.Count != 1 || lastCards.Count != 1)
                return false; // 只有一组
            var card2 = lastCards.First();
            return card1.Amount > card2.Amount && // 大小比较
                   card1.Count == count &&
                   card2.Count == count;
        }
    }
}