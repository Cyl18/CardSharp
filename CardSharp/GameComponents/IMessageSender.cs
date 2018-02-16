namespace CardSharp
{
    public interface IMessageSender
    {
        void AddMessage(string msg);
        void ClearMessage();
    }
}