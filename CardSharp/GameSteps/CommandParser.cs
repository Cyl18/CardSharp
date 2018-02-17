using System;
using System.Diagnostics;
using System.Linq;

namespace CardSharp.GameSteps
{
    public class CommandParser : Samsara, ICommandParser
    {
        public CommandParser(Desk desk)
        {
            CurrentIndex = desk.PlayerList.FindIndex(p => p.Type == PlayerType.Landlord);
        }

        public void Prepare(Desk desk)
        {
            desk.AddMessage($"请{desk.CurrentPlayer.ToAtCode()}出牌");
            RunHostedCheck(desk);
        }

        public void Parse(Desk desk, Player player, string command)
        {
            if (!desk.Players.Contains(player))
                return;
            if (desk.LastSuccessfulSender == desk.CurrentPlayer)
            {
                desk.CurrentRule = null;
                desk.LastCards = null;
            }

            switch (command)
            {
                case "结束游戏":
                    desk.AddMessage("CNM");
                    desk.FinishGame();
                    break;
                case "记牌器":
                    desk.AddMessage(CardCounter.GenerateCardString(desk));
                    break;
                case "全场牌数":
                    desk.AddMessage(string.Join(Environment.NewLine,
                        desk.PlayerList.Select(p => $"{p.ToAtCode()}: {p.Cards.Count}")));
                    break;
                case "弃牌":
                    player.GiveUp = true;
                    desk.AddMessage("弃牌成功");
                    return;
                case "托管":
                    player.HostedEnabled = true;
                    desk.AddMessage("托管成功");
                    RunHostedCheck(desk);
                    return;
                case "结束托管":
                    player.HostedEnabled = false;
                    desk.AddMessage("结束成功");
                    break;
            }

            if (!IsValidPlayer(desk, player))
                return;

            switch (command)
            {
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
                case "passssssssssss": // 应LG的要求 别怪我 这里是暴力写法
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
                    if (desk.CurrentRule == null)
                    {
                        desk.AddMessage("为什么会这样呢...为什么你不出牌呢...");
                    }
                    else
                    {
                        AnalyzeGiveUpAndMoveNext(desk);
                        desk.BoardcastCards();
                    }

                    break;
            }

            if (command.StartsWith("出"))
            {
                var cardsCommand = command.Substring(1).ToUpper();
                if (cardsCommand.IsValidCardString())
                    if (Rules.Rules.IsCardsMatch(cardsCommand.ToCards(), desk))
                    {
                        player.SendCards(desk);
                        if (player.Cards.Count <= Constants.BoardcastCardNumThreshold)
                            desk.AddMessageLine($"{player.ToAtCode()} 只剩{player.Cards.Count}张牌啦~");

                        if (desk.SuddenDeathEnabled) desk.AddMessageLine("WARNING: SUDDEN DEATH ENABLED");

                        if (player.PublicCards) desk.AddMessageLine($"明牌:{player.Cards.ToFormatString()}");

                        AnalyzeGiveUpAndMoveNext(desk);

                        if (desk.LastSuccessfulSender == desk.CurrentPlayer)
                        {
                            desk.CurrentRule = null;
                            desk.LastCards = null;
                        }

                        desk.BoardcastCards();
                    }
                    else
                    {
                        desk.AddMessage("无法匹配到你想出的牌哟~");
                    }
            }

            if (desk.CurrentPlayer.Cards.Count == 0) {
                PlayerWin(desk, player);
                return;
            }

            if (RunHostedCheck(desk))
                return;
            
            RunAutoPassCheck(desk);
        }

        private void RunAutoPassCheck(Desk desk)
        {
            var cp = desk.CurrentPlayer;
            var (exists, _) = Rules.Rules.FirstMatch(cp, desk);

            if (!exists)
            {
                desk.AddMessageLine($"{cp.ToAtCode()} 没有检测到你想要出的牌, 已为你自动pass.");
                Parse(desk, cp, "pass");
            }
        }

        private bool RunHostedCheck(Desk desk)
        {
            var cp = desk.CurrentPlayer;
            if (!cp.HostedEnabled) return false;

            if (desk.LastSuccessfulSender == cp)
            {
                desk.CurrentRule = null;
                desk.LastCards = null;
            }

            var (exists, cards) = Rules.Rules.FirstMatch(cp, desk);
#if DEBUG
            if (new StackTrace().FrameCount > 500)
            {
                Debugger.Break();
            }
#endif
            switch (exists)
            {
                case true:
                    desk.AddMessageLine($" {cp.ToAtCode()} 托管出牌 {cards.ToFormatString()}");
                    Parse(desk, cp, $"出{string.Join("", cards.Select(card => card.ToString()))}");
                    return true;
                case false:
                    desk.AddMessageLine($" {cp.ToAtCode()} 托管过牌");
                    Parse(desk, cp, "pass");
                    return true;
            }

            return true;
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
    }
}