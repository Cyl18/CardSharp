using System;
using System.Linq;
using System.Threading;

namespace CardSharp.GameSteps
{
    public class CommandParser : Samsara, ICommandParser
    {
        public void Parse(Desk desk, Player player, string command)
        {
            if (!desk.Players.Contains(player))
                return;
            if (desk.LastSuccessfulSender == desk.CurrentPlayer) {
                desk.CurrentRule = null;
                desk.LastCards = null;
            }

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
                case "弃牌":
                    player.GiveUp = true;
                    desk.AddMessage("弃牌成功");
                    return;
                case "托管":
                    player.HostedEnabled = true;
                    desk.AddMessage("托管成功");
                    goto hosted;
            }

            if (!IsValidPlayer(desk, player))
                return;

            switch (command) {
                case "过":
                #region Pass
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
                #endregion
                case "不出":
                case "不要":
                case "出你妈":
                case "要你妈":
                    if (desk.CurrentRule == null) {
                        desk.AddMessage("为什么会这样呢...为什么你不出牌呢...");
                    } else {
                        AnalyzeGiveUpAndMoveNext(desk);
                        desk.BoardcastCards();
                    }

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
                            desk.AddMessageLine($"{player.ToAtCode()} 只剩{player.Cards.Count}张牌啦~");
                        }

                        if (desk.SuddenDeathEnabled)
                        {
                            desk.AddMessageLine("WARNING: SUDDEN DEATH ENABLED");
                        }

                        if (player.PublicCards)
                        {
                            desk.AddMessageLine($"明牌:{player.Cards.ToFormatString()}");
                        }

                        AnalyzeGiveUpAndMoveNext(desk);

                        if (desk.LastSuccessfulSender == desk.CurrentPlayer) {
                            desk.CurrentRule = null;
                            desk.LastCards = null;
                        }

                        desk.BoardcastCards();
                    } else {
                        desk.AddMessage("无法匹配到你想出的牌哟~");
                    }
            }


            hosted:
            var cp = desk.CurrentPlayer;
            if (!cp.HostedEnabled) return;

            var result = Rules.Rules.FirstMatch(cp, desk);
            Thread.Sleep(10);
            switch (result.exists) {
                case true:
                    Parse(desk, cp, $"出{string.Join("", result.cards.Select(card => card.ToString()))}");
                    desk.AddMessageLine();
                    return;
                case false:
                    Parse(desk, cp, "pass");
                    desk.AddMessageLine();
                    return;
            }
        }

        private static void PlayerWin(Desk desk, Player player)
        {
            desk.FinishGame(player);

        }

        private void AnalyzeGiveUpAndMoveNext(Desk desk)
        {
            do
            {
                MoveNext();
            } while (desk.CurrentPlayer.GiveUp);

            var farmers = desk.Players.Where(p => p.Type == PlayerType.Farmer);
            var landlords = desk.Players.Where(p => p.Type == PlayerType.Landlord);
            if (farmers.All(p => p.GiveUp) || landlords.All(p => p.GiveUp))
                desk.FinishGame(desk.Players.First(p => !p.GiveUp));

            
        }

        public CommandParser(Desk desk)
        {
            CurrentIndex = desk.PlayerList.FindIndex(p => p.Type == PlayerType.Landlord);
            desk.AddMessage($"请{desk.GetPlayerFromIndex(CurrentIndex).ToAtCode()}出牌");
        }
    }
}