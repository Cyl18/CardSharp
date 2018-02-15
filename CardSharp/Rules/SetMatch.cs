using System.Collections.Generic;
using System.Linq;

namespace CardSharp.Rules
{
    public static class SetMatch
    {
        public static bool IsMatch(List<Card> cards, List<Card> lastCards, int num)
        {
            if (cards.Count != num)
                return false;
            var source = cards.ToSet();
            if (source.Count != 1)
                return false;

            if (lastCards == null)
                return true;
            var dest = lastCards.ToSet();

            return source.First().Amount > dest.First().Amount;
        }
    }
}