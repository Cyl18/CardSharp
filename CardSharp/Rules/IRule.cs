using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp.Rules
{
    public interface IRule
    {
        bool IsMatch(List<CardGroup> cards, List<CardGroup> lastCards);
        string ToString(List<CardGroup> cards);
    }
}
