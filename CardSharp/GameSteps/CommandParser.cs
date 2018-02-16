using System;
using System.Linq;

namespace CardSharp.GameSteps
{
    public class CommandParser : Samsara, ICommandParser
    {
        public void Parse(Desk desk, Player player, string command)
        {
            if (!desk.Players.Contains(player))
                return;
            switch (command) {
                case "结束游戏":
                    desk.AddMessage("CNM");
                    desk.FinishGame();
                    break;
                case "记牌器":
                    desk.AddMessage(CardCounter.GenerateCardString(desk));
                    break;
                case "全场牌数":
                    desk.AddMessage(string.Join(Environment.NewLine, desk.PlayerList.Select(p => $"{p.ToAtCode()}: {p.Cards.Count}")));
                    break;
            }

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
                        desk.AddMessage("为什么会这样呢...为什么你不出牌呢...");
                    } else {
                        MoveNext();
                        desk.BoardcastCards();
                    }
                    return;
                case "弃牌":
                    player.GiveUp = true;
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
                        if (player.Cards.Count <= Constants.BoardcastCardNumThreshold) {
                            desk.AddMessage($"{player.ToAtCode()} 只剩{player.Cards.Count}张牌啦~{Environment.NewLine}");
                        }// TODO 请开始你的表演
                        MoveNext();
                        if (desk.LastSuccessfulSender == desk.CurrentPlayer) {
                            desk.CurrentRule = null;
                            desk.LastCards = null;
                        }
                        desk.BoardcastCards();
                    } else {
                        desk.AddMessage("无法匹配到你想出的牌哟~");
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