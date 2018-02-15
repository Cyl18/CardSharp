namespace CardSharp
{
    public interface ICommandParser
    {
        string Parse(Desk desk, Player player, string command);
    }
}