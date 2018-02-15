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
            var amount = obj as CardAmount?;
            return Amount == amount?.Amount;
        }

        public static implicit operator int(CardAmount amount)
        {
            return amount.Amount;
        }

        public static implicit operator CardAmount(int amount)
        {
            return new CardAmount(amount);
        }
    }
}
