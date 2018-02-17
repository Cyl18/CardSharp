namespace CardSharp
{
    public struct CardAmount
    {
        public CardAmount(int amount) : this()
        {
            Amount = amount;
        }

        public int Amount { get; }


        public override bool Equals(object obj)
        {
            return obj != null && Amount == ((CardAmount) obj).Amount;
        }

        public override int GetHashCode()
        {
            return Amount;
        }

        public static implicit operator int(CardAmount amount)
        {
            return amount.Amount;
        }

        public static implicit operator CardAmount(int amount)
        {
            return new CardAmount(amount);
        }

        public static bool operator ==(CardAmount card1, CardAmount card2)
        {
            return card1.Equals(card2);
        }

        public static bool operator !=(CardAmount card1, CardAmount card2)
        {
            return !(card1 == card2);
        }
    }
}