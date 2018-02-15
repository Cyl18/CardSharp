namespace CardSharp
{
    public interface ICard
    {
        CardAmount Amount { get; }
        CardType Type { get; }
    }
}