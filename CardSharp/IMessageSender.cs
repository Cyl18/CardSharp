namespace CardSharp
{
    internal interface IMessageSender
    {
        void SendGroupMessage(string id, string message);
        void SendPrivateMessage(string id, string message);
    }
}