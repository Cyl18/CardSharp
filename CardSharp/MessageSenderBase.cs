using System;
using System.Runtime.CompilerServices;

namespace CardSharp
{
    public abstract class MessageSenderBase : IMessageSender
    {
        private readonly object _locker = new object();
        public string Message { get; private set; }

        public virtual void AddMessage(string msg)
        {
            lock (_locker)
            {
                Message += msg;
            }
        }

        public void ClearMessage()
        {
            lock (_locker)
            {
                Message = null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddMessageLine(string msg = "")
        {
            if (!Message.EndsWith(Environment.NewLine))
            {
                AddMessage(Environment.NewLine);
            }
            AddMessage(msg + Environment.NewLine);
        }
    }
}