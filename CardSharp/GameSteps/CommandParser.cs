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
            desk.AddMessage($"请{desk.CurrentPlayer.ToAtCodeWithRole()}出牌");
            RunHostedCheck(desk);
        }

        public void Parse(Desk desk, Player player, string command)
        {
            if (ParseInternal(desk, player, command)) return;

            if (!IsValidPlayer(desk, desk.CurrentPlayer)) return;
            if (RunHostedCheck(desk))
                return;
            if (desk.CurrentRule != null)
                RunAutoPassCheck(desk);
        }

        private bool ParseInternal(Desk desk, Player player, string command)
        {
            if (!desk.Players.Contains(player)) return true;

            if (ParseStandardCommand(desk, player, command)) return true;

            RefreshCurrentRule(desk);

            if (!IsValidPlayer(desk, player))
                return true;

            ParsePassCommand(desk, command);

            ParsePlayerSubmitCard(desk, player, command);
            RefreshCurrentRule(desk);
            if (CheckPlayerWin(desk)) return true;

            return false;
        }

        private void ParsePlayerSubmitCard(Desk desk, Player player, string command)
        {
            if (command.StartsWith("出"))
            {
                var cardsCommand = command.Substring(1).ToUpper();
                if (cardsCommand.IsValidCardString())
                    if (Rules.Rules.IsCardsMatch(cardsCommand.ToCards(), desk))
                    {
                        player.SendCards(desk);
                        if (CheckPlayerWin(desk))  return;
                        if (player.Cards.Count <= Constants.BoardcastCardNumThreshold)
                            desk.AddMessageLine($"{player.ToAtCodeWithRole()} 只剩{player.Cards.Count}张牌啦~");

                        if (desk.SuddenDeathEnabled) desk.AddMessageLine("WARNING: SUDDEN DEATH ENABLED");

                        if (player.PublicCards) desk.AddMessageLine($"明牌:{player.Cards.ToFormatString()}");

                        AnalyzeGiveUpAndMoveNext(desk);

                        desk.BoardcastCards();
                    }
                    else
                    {
                        desk.AddMessage("你似乎不能出这些牌哟~");
                    }
            }
        }

        private static bool CheckPlayerWin(Desk desk)
        {
            if (desk.CurrentPlayer.Cards.Count == 0 && desk.State != GameState.Unknown)
            {
                PlayerWin(desk, desk.CurrentPlayer);
                return true;
            }

            return false;
        }

        private void ParsePassCommand(Desk desk, string command)
        {
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
        }

        private static void RefreshCurrentRule(Desk desk)
        {
            if (desk.LastSuccessfulSender == desk.CurrentPlayer)
            {
                desk.CurrentRule = null;
                desk.LastCards = null;
            }
        }

        private bool ParseStandardCommand(Desk desk, Player player, string command)
        {
            switch (command)
            {
                case "结束游戏":
                    desk.AddMessage("请寻找管理员结束.");
                    return true;
                case "记牌器":
                    desk.AddMessage(CardCounter.GenerateCardString(desk));
                    return true;
                case "全场牌数":
                    desk.AddMessage(string.Join(Environment.NewLine,
                        desk.PlayerList.Select(p => $"{p.ToAtCodeWithRole()}: {p.Cards.Count}")));
                    return true;
                case "弃牌":
                    player.GiveUp = true;
                    desk.AddMessage("弃牌成功");
                    return true;
                case "托管":
                    player.HostedEnabled = true;
                    desk.AddMessage("托管成功");
                    RunHostedCheck(desk);
                    return true;
                case "结束托管":
                    player.HostedEnabled = false;
                    desk.AddMessage("结束成功");
                    return true;
            }

            return false;
        }

        private void RunAutoPassCheck(Desk desk)
        {
            var cp = desk.CurrentPlayer;
            if (desk.State == GameState.Unknown) return;
            
            var (exists, _) = Rules.Rules.FirstMatch(cp, desk);

            if (!exists)
            {
                if (cp.AutoPass)
                {
                    Parse(desk, cp, "pass");
                }
                else
                {
                    cp.AddMessage("没有检测到你能出的牌, 你可以pass.");
                }
            }
        }

        private bool RunHostedCheck(Desk desk)
        {
            var cp = desk.CurrentPlayer;
            if (!cp.HostedEnabled || desk.State == GameState.Unknown) return false;

            RefreshCurrentRule(desk);

            var (exists, cards) = Rules.Rules.FirstMatch(cp, desk);
            switch (exists)
            {
                case true:
                    desk.AddMessageLine($" {cp.ToAtCodeWithRole()} 托管出牌 {cards.ToFormatString()}");
                    Parse(desk, cp, $"出{string.Join("", cards.Select(card => card.ToString()))}");
                    return true;
                case false:
                    desk.AddMessageLine($" {cp.ToAtCodeWithRole()} 托管过牌");
                    Parse(desk, cp, "pass");
                    return true;
            }

            return true;
        }

        private static void PlayerWin(Desk desk, Player player)
        {
            if (desk.State == GameState.Unknown) return;
            desk.FinishGame(player);
        }

        private void AnalyzeGiveUpAndMoveNext(Desk desk)
        {
            if (desk.State == GameState.Unknown)
            {
                return;
            }

            do
            {
                MoveNext();
            } while (desk.CurrentPlayer.GiveUp);

            CheckGameFinished(desk);
        }

        private static bool CheckGameFinished(Desk desk)
        {
            
            var farmers = desk.Players.Where(p => p.Type == PlayerType.Farmer);
            var landlords = desk.Players.Where(p => p.Type == PlayerType.Landlord);
            if (farmers.All(p => p.GiveUp) || landlords.All(p => p.GiveUp))
            {
                desk.FinishGame(desk.Players.First(p => !p.GiveUp));
                return true;
            }

            if (desk.CurrentPlayer.Cards.Count == 0) {
                PlayerWin(desk, desk.CurrentPlayer);
                return true;
            }

            return false;
        }
    }
}