using System.Linq;

namespace CardSharp.GameSteps
{
    public class CommandParser : Samsara, ICommandParser
    {
        public void Parse(Desk desk, Player player, string command)
        {
            if (!IsValidPlayer(desk, player))
                return;

            if (desk.LastSuccessfulSender == desk.CurrentPlayer) {
                desk.CurrentRule = null;
                desk.LastCards = null;
            }

            if (command.StartsWith("出")) {
                var cardsCommand = command.Substring(1).ToUpper();
                if (cardsCommand.IsValidCardString())
                    if (Rules.Rules.IsCardsMatch(cardsCommand.ToCards(), desk)) {
                        if (player.Cards.Count == 0) {
                            PlayerWin(desk, player);
                            return;
                        }

                        player.SendCards(desk);
                        MoveNext();
                        if (desk.LastSuccessfulSender == desk.CurrentPlayer) {
                            desk.CurrentRule = null;
                            desk.LastCards = null;
                        }
                        desk.BoardcastCards();
                    } else {
                        desk.AddMessage("匹配失败");
                    }
            }

            switch (command) {
                case "过":
                case "pass":
                case "不出":
                case "不要":
                    if (desk.CurrentRule == null) {
                        desk.AddMessage("你必须出牌");
                    } else {
                        MoveNext();
                        desk.BoardcastCards();
                    }
                    break;
                case "结束游戏":
                    desk.AddMessage("CNM");
                    desk.FinishGame();
                    break;
            }
        }

        private static void PlayerWin(Desk desk, Player player)
        {
            desk.FinishGame(player);
            
        }

        public CommandParser(Desk desk)
        {
            CurrentIndex = desk.PlayerList.FindIndex(p => p.Type == PlayerType.Landlord);
            desk.AddMessage($"请{desk.GetPlayerFromIndex(CurrentIndex).ToAtCode()}出牌");
        }
    }
}