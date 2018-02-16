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

            switch (command) {
                case "过":
                case "pass":
                case "passs":
                case "passss":
                case "passsss":
                case "passssss":
                case "passsssss":
                case "passssssss":
                case "passsssssss":
                case "passssssssss":
                case "passsssssssss":
                case "passssssssssss":       // 应LG的要求 别怪我 这里是暴力写法
                case "passsssssssssss":
                case "passssssssssssss":
                case "passsssssssssssss":
                case "passssssssssssssss":
                case "passsssssssssssssss":
                case "passssssssssssssssss":
                case "passsssssssssssssssss":
                case "passssssssssssssssssss":
                case "passsssssssssssssssssss":
                case "不出":
                case "不要":
                case "出你妈":
                case "要你妈":
                    if (desk.CurrentRule == null) {
                        desk.AddMessage("你必须出牌");
                    } else {
                        MoveNext();
                        desk.BoardcastCards();
                    }
                    return;
                case "结束游戏":
                    desk.AddMessage("CNM");
                    desk.FinishGame();
                    break;
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
                        if (player.Cards.Count <= Constants.BoardcastCardNumThreshold)
                        {
                            desk.AddMessage($"{player.ToAtCode()} 还剩{player.Cards.Count}张牌");
                        }
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