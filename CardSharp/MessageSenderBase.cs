using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CardSharp
{
    public abstract class MessageSenderBase : IMessageSender
    {
        private volatile string _message;
        private readonly ReaderWriterLockSlim _rwlock = new ReaderWriterLockSlim();

        public string Message
        {
            get
            {
                _rwlock.EnterReadLock();
                var str = _message;
                _rwlock.ExitReadLock();
                return str;
            }
            set
            {
                _rwlock.EnterWriteLock();
                _message = value;
                _rwlock.ExitWriteLock();
            }
        }

        public virtual void AddMessage(string msg)
        {
            Message += msg;
        }

        public void ClearMessage()
        {
            Message = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddMessageLine(string msg = "")
        {
            if (Message?.EndsWith(Environment.NewLine) == false)
            {
                AddMessage(Environment.NewLine);
            }
            AddMessage(msg + Environment.NewLine);
        }
    }
}