using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardSharp.I18N;
using Newtonsoft.Json.Linq;

namespace CardSharp.Rules
{
    public abstract class RuleBase : IRule
    {
        public abstract bool IsMatch(List<CardGroup> cardGroups, List<CardGroup> lastCardGroups);

        public override string ToString()
        {
            return ((IEnumerable<JToken>) Localization.Normal.rules) // TODO PLZ
                .Select(token => token.ToString()).First(name => name == this.GetType().Name);
        }

        public abstract (bool exists, List<Card> cards) FirstMatchedCards(List<CardGroup> sourceGroups,
            List<CardGroup> lastCardGroups);
    }
}