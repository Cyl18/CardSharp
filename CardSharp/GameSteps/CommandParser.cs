using System.Linq;

namespace CardSharp.GameSteps
{
    public class CommandParser : Samsara, ICommandParser
    {
        public void Parse(Desk desk, Player player, string command)
        {
            if (!IsValidPlayer(desk, player))
                return;
            

            if (command.StartsWith("出")) {
                var cardsCommand = command.Substring(1).ToUpper();
                if (cardsCommand.IsValidCardString())
                    if (Rules.Rules.IsCardsMatch(cardsCommand.ToCards(), desk)) {
                        if (player.Cards.Count == 0) {
                            desk.AddMessage($"{player.ToAtCode()}赢了.");
                            desk.FinishGame();
                            return;
                        }

                        player.AddMessage(desk.DeskId + string.Join(string.Empty, player.Cards.Select(card => $"[{card}]")));
                        MoveNext();
                        if (desk.LastSuccessfulSender == desk.CurrentPlayer) {
                            desk.CurrentRule = null;
                            desk.LastCards = null;
                        }
                        SendCards(desk);
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
                        SendCards(desk);
                    }
                    break;
                case "结束游戏":
                    desk.AddMessage("CNM");
                    desk.FinishGame();
                    break;
            }
        }

        private void SendCards(Desk desk)
        {
            desk.AddMessage($"{desk.CurrentRule.ToString()}-{string.Join(string.Empty, desk.LastCards.Select(card => $"[{card}]"))} {desk.CurrentPlayer.ToAtCode()}请出牌");
        }

        public CommandParser(Desk desk)
        {
            CurrentIndex = desk.PlayerList.FindIndex(p => p == desk.Landlord);
            desk.AddMessage($"请{desk.Landlord.ToAtCode()}出牌");
        }
    }
}