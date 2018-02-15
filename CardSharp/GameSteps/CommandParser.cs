using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp.GameSteps
{
    public class CommandParser : Samsara, ICommandParser
    {
        public string Prepare(Desk desk)
        {
            CurrentIndex = desk.PlayerList.FindIndex(p => p == desk.Landlord);
            return $"请{desk.Landlord.ToAtCode()}出牌";
        }

        public string Parse(Desk desk, Player player, string command)
        {
            if (!IsValidPlayer(desk, player)) return null;
            
            if (command.StartsWith("出"))
            {
                var cardsCommand = command.Substring(1).ToUpper();
                if (cardsCommand.IsValidCardString())
                {
                    if (Rules.Rules.IsCardsMatch(cardsCommand.ToCards(), desk))
                    {
                        MoveNext();
                        return $"{desk.CurrentRule}. {player.ToAtCode()}请出牌";
                    }
                    else
                    {
                        return "匹配失败";
                    }
                }
            }

            return null;
            //TODO
        }
    }
}
