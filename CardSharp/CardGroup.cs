using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
