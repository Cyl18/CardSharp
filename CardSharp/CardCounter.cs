using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardSharp
{
    public class CardCounter
    {
        public static string GenerateCardString(Desk desk)
        {
            var allcards = new List<Card>();
            foreach (var cards in desk.PlayerList.Select(player => player.Cards))
                allcards.AddRange(cards);
            allcards.Sort();
            var dcards = Desk.GenerateCards();

            var allcardsGroups = allcards.ExtractCardGroups(true);
            var dcardGroups = dcards.ExtractCardGroups();
            var counts = new int[dcardGroups.Count];
            for (var index = 0; index < dcardGroups.Count; index++)
            {
                var dcardGroup = dcardGroups[index];
                var allcardGroup = allcardsGroups[index];
                counts[index] = dcardGroup.Count - (dcardGroup.Count - allcardGroup.Count);
            }

            var sb = new StringBuilder();
            for (var index = 0; index < counts.Length; index++)
            {
                var count = counts[index];
                sb.AppendLine($"[{new Card(index)}]: {count}");
            }

            return sb.ToString();
        }
    }
}