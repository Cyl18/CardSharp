namespace CardSharp.GameSteps
{
    public class CommandParser : Samsara, ICommandParser
    {
        public string Parse(Desk desk, Player player, string command)
        {
            if (!IsValidPlayer(desk, player)) return null;

            if (command.StartsWith("出"))
            {
                var cardsCommand = command.Substring(1).ToUpper();
                if (cardsCommand.IsValidCardString())
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

            return null;
            //TODO
        }

        public string Prepare(Desk desk)
        {
            CurrentIndex = desk.PlayerList.FindIndex(p => p == desk.Landlord);
            return $"请{desk.Landlord.ToAtCode()}出牌";
        }
    }
}