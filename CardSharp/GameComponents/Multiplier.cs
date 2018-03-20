using System;
using CardSharp.Rules;

namespace CardSharp.GameComponents
{
    public static class Multiplier
    {
        public static void Multiplie(Desk desk, IRule rule)
        {
            switch (rule)
            {
                case RuleBomb _:
                    desk.Multiplier += 1;
                    break;
                case RuleRocket _:
                    desk.Multiplier += 2;
                    break;
            }
        }

        public static long CalcResult(Desk desk)
        {
            return (long) Math.Pow(2, desk.Multiplier) * Constants.BaseScore;
        }
    }
}