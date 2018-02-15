using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp
{
    public static class CardGroupExtionsions
    {
        public static int CountCardCountMatches(this IEnumerable<CardGroup> groups, int num)
        {
            return groups.Count(cardGroup => cardGroup.Count == num);
        }

        public static Card GetFirstCardFromCount(this IEnumerable<CardGroup> groups, int num)
        {
            return new Card(groups.First(cardGroup => cardGroup.Count == num).Amount);
        }

        public static List<CardGroup> Update(this List<CardGroup> groups)
        {
            for (var index = 0; index < groups.Count; index++)
            {
                var cardGroup = groups[index];
                if (cardGroup.Count == 0)
                {
                    groups.RemoveAt(index);
                    index--;
                }
            }

            return groups;
        }
    }
}
