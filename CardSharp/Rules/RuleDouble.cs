using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp.Rules
{
    /// <summary>
    /// 33
    /// </summary>
    public class RuleDouble : RuleBase
    {
        public override bool IsMatch(List<CardGroup> cards, List<CardGroup> lastCards)
        {
            return SingleGroupMatch.IsMatch(cards, lastCards, 2);
        }

        public override string ToString(List<CardGroup> cards)
        {
            return "对子";
        }
    }
}
