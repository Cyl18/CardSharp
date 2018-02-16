namespace CardSharp
{
    public interface ICommandParser
    {
        void Parse(Desk desk, Player player, string command);
    }
}