namespace CardSharp
{
    public interface IMessageSender
    {
        string Message { get; }
        void AddMessage(string msg);
        void ClearMessage();
    }
}