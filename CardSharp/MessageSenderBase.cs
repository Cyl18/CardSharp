using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp
{
    public abstract class MessageSenderBase : IMessageSender
    {
        public string Message { get; private set; }
        private readonly object _locker = new object();

        public void AddMessage(string msg)
        {
            lock (_locker) {
                Message += msg;
            }
        }

        public void AddMessageLine(string msg = "")
        {
            lock (_locker) {
                Message += msg + Environment.NewLine;
            }
        }

        public void ClearMessage()
        {
            lock (_locker) {
                Message = null;
            }
        }
    }
}
