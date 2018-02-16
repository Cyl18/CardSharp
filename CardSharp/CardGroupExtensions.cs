using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp
{
    public static class CardGroupExtensions
    {
        public static IEnumerable<Card> ToCards(this IEnumerable<CardGroup> cardGroups)
        {
            foreach (var cardGroup in cardGroups)
                for (var i = 0; i < cardGroup.Count; i++)
                    yield return new Card(cardGroup.Amount);
            
        }
    }
}
