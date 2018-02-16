using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CardSharp
{
    public class Card : ICard, IComparable<Card>, IEquatable<Card>
    {
        public Card(CardAmount amount)
        {
            Amount = amount;
            Type = amount.ToType();
        }

        public CardType Type { get; }
        public CardAmount Amount { get; }

        public int CompareTo(Card other)
        {
            return ((int) Amount).CompareTo(other.Amount);
        }

        public bool Equals(Card other)
        {
            return other != null &&
                   EqualityComparer<CardAmount>.Default.Equals(Amount, other.Amount);
        }

        public override int GetHashCode()
        {
            return Amount.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Card);
        }

        public override string ToString()
        {
            const int px = 3;
            const int nmax = 10 - px + 1;
            if (Amount < nmax)
                return (px + Amount).ToString(); //3456789[10]
            switch (Type)
            {
                case CardType.Amount:
                    switch (Amount.Amount)
                    {
                        case Constants.Cards.CJ:
                            return "J";
                        case Constants.Cards.CQ:
                            return "Q";
                        case Constants.Cards.CK:
                            return "K";
                        case Constants.Cards.CA:
                            return "A";
                        case Constants.Cards.C2:
                            return "2";
                        default:
                            throw new IndexOutOfRangeException("Not supported!");
                    }
                case CardType.King:
                    switch (Amount.Amount)
                    {
                        case Constants.Cards.CGhost:
                            return "鬼";
                        case Constants.Cards.CKing:
                            return "王";
                        default:
                            throw new IndexOutOfRangeException("Not supported!");
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool operator ==(Card card1, Card card2)
        {
            return EqualityComparer<Card>.Default.Equals(card1, card2);
        }

        public static bool operator !=(Card card1, Card card2)
        {
            return !(card1 == card2);
        }
    }

    public static class CardExtensions
    {
        public static CardType ToType(this CardAmount card)
        {
            return card.Amount >= Constants.AmountCardMax ? CardType.King : CardType.Amount;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<CardGroup> ExtractCardGroups(this List<Card> cards)
        {
            return ExtractCardGroupsInternal(cards);
        }

        public static (bool isVaild, List<Card> result) IsTargetVaildAndRemove(this List<Card> cards, List<Card> target)
        {
            var cardGroups = cards.ExtractCardGroups();
            var targetGroups = target.ExtractCardGroups();
            foreach (var targetGroup in targetGroups)
            {
                var group = cardGroups.FirstOrDefault(g => g.Amount == targetGroup.Amount);
                if (group == null) return (false, null);
                group.Count -= targetGroup.Count;
                if (group.Count < 0)
                    return (false, null);
            }

            cardGroups.RemoveAll(cardgroup => cardgroup.Count == 0); // remove all 0
            return (true, cardGroups.ToCards().ToList());
        }

        public static string ToFormatString(this IEnumerable<Card> cards)
        {
            return string.Join(string.Empty, cards.Select(card => $"[{card}]"));
        }

        // MUST SORT
        private static unsafe List<CardGroup> ExtractCardGroupsInternal(this List<Card> cards)
        {
            var enumerable = cards;
            var cardnums = enumerable.Select(card => (int) card.Amount);
            var length = enumerable.Last().Amount + 1;
            var array = stackalloc int[length];
            SetAll0(array, length);
            foreach (var num in cardnums) array[num]++;
            var o = new List<CardGroup>(length);
            for (var i = 0; i < length; i++)
            {
                var num = array[i];
                if (num != 0) o.Add(new CardGroup(i, num));
            }

            return o;

            void SetAll0(int* source, int len)
            {
                for (var i = 0; i < len; i++) source[i] = 0;
            }
        }
    }
}