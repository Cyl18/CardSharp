namespace CardSharp
{
    public class CardGroup
    {
        public CardGroup(int amount, int count)
        {
            Count = count;
            Amount = amount;
        }

        public int Amount { get; }
        public int Count { get; }
    }
}