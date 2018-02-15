using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp.Rules
{
    public static class Rules
    {
        private static readonly List<IRule> RulesList = new List<IRule>
        {
            new RuleSingle(),    // 单张   => 3
            new RuleDouble(),    // 对子   => 33
            new Rule3(),         // 三张   => 333
            new Rule3With1(),    // 三带一 => 3334
            new Rule3With2(),    // 三带二 => 33344
            new Rule4With2(),    // 四带二 => 333344
            new Rule4With4(),    // 四带四 => 33334455
            new RuleAirplane(),  // 飞机   => 333444
            new RuleAirplain1(), // 小飞机 => 33344456
            new RuleAirplain2(), // 大飞机 => 3334445566
            new RuleChain(),     // 顺子   => 3456789
            new RuleChain2(),    // 双顺   => 33445566
            new RuleBomb(),      // 炸弹   => 3333
            new RuleRocket()     // 火箭   => 这还用说?
        };

        public static bool IsCardsMatch(IEnumerable<Card> cards, Desk desk)
        {
            var list = cards.ToListAndSort();
            if (desk.LastSuccessfulSender == desk.CurrentPlayer)
                desk.CurrentRule = null;
            if (list.Count == 0)
                return false;

            var lastCardList = desk.LastCards?.ToList();

            foreach (var rule in RulesList) {
                if (desk.CurrentRule == null || desk.CurrentRule == rule || rule is RuleBomb || rule is RuleRocket) {
                    if (desk.CurrentRule is RuleRocket && rule is RuleBomb)
                        continue;
                    if (rule.IsMatch(list.ExtractCardGroups(), lastCardList?.ExtractCardGroups())) {
                        desk.LastSuccessfulSender = desk.CurrentPlayer;
                        desk.CurrentRule = rule;
                        desk.LastCards = list;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
