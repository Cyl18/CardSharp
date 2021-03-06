﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CardSharp.GameComponents;
using CardSharp.GameSteps;

namespace CardSharp
{
    public class LandlordDiscuss : Samsara, ICommandParser
    {
        private readonly IEnumerable<Card> _landlordCards;
        private int _count;

        public LandlordDiscuss(IEnumerable<Card> landlordCards, Desk desk)
        {
            _landlordCards = landlordCards;
            var player = desk.GetPlayerFromIndex(CurrentIndex);
            desk.AddMessage($"开始游戏, {player.ToAtCode()}你要抢地主吗?");
        }

        public void Prepare(Desk desk)
        {
#if DEBUG
            if (desk.CurrentPlayer is FakePlayer) {
                Parse(desk, desk.CurrentPlayer, "抢");
            }
#else
            if (desk.CurrentPlayer is FakePlayer) {
                Parse(desk, desk.CurrentPlayer, "抢");
            }
#endif
        }

        public void Parse(Desk desk, Player player, string command)
        {
            if (!desk.Players.Contains(player))
                return;
            switch (command) {
                case "加倍":
                    if (desk.Players.Any(p => p is FakePlayer)) {
                        desk.AddMessage("有机器人玩家, 加倍不可用");
                        break;
                    }

                    if (!player.Multiplied) {
                        desk.AddMessage("好的。");
                        desk.Multiplier += 1;
                        player.Multiplied = true;
                    }

                    break;
                case "超级加倍":
                    if (desk.Players.Any(p => p is FakePlayer)) {
                        desk.AddMessage("有机器人玩家, 加倍不可用");
                        break;
                    }

                    if (!player.Multiplied) {
                        desk.AddMessage("您牛逼。");
                        desk.Multiplier += 2;
                        player.Multiplied = true;
                    }
                    
                    break;
                case "减倍":
                    if (desk.Players.Any(p => p is FakePlayer))
                    {
                        desk.AddMessage("有机器人玩家, 减倍不可用");
                        break;
                    }

                    if (!player.Multiplied)
                    {
                        desk.AddMessage("减倍完成.");
                        desk.Multiplier -= 1;
                        player.Multiplied = true;
                    }

                    break;
                case "超级减倍":
                    if (desk.Players.Any(p => p is FakePlayer))
                    {
                        desk.AddMessage("有机器人玩家, 加倍不可用");
                        break;
                    }

                    if (!player.Multiplied)
                    {
                        desk.AddMessage("您牛逼.");
                        desk.Multiplier -= 2;
                        player.Multiplied = true;
                    }

                    break;
                case "SUDDEN_DEATH_DUEL_CARD":
                    if (desk.Players.Any(p => p is FakePlayer)) {
                        desk.AddMessage("有机器人玩家, 加倍不可用");
                        break;
                    }
                    if (!player.Multiplied && !desk.SuddenDeathEnabled) {
                        desk.AddMessage("SUDDEN DEATH ENABLED.");
                        desk.SuddenDeathEnabled = true;
                        player.Multiplied = true;
                    }

                    break;
                case "明牌":
                    if (!player.PublicCards) {
                        player.PublicCards = true;
                        desk.Multiplier += 1;
                        desk.AddMessage("您可真牛逼。");
                    }

                    break;
                case "结束游戏":
                    desk.FinishGame();
                    return;
            }

            if (!IsValidPlayer(desk, player))
                return;

            if (desk.State == GameState.Unknown) return;

            switch (command) {
                case "y":
                case "抢":
                case "抢地主":
                case "抢他妈的":
                case "抢这个鸡毛掸子": // 应irol的要求. 开心就好啦.
                    player.Cards.AddRange(_landlordCards);
                    player.Cards.Sort();
                    desk.AddMessage(
                        $"{player.ToAtCode()}抢了地主. 底牌有{string.Join("", _landlordCards.Select(card => $"[{card}]"))}");
                    desk.SetLandlord(player);
                    desk.SendCardsMessage();
                    break;
                case "n":
                case "不":
                case "不抢":
                case "抢你妈":
                case "抢个鸡毛掸子": // 应LG的要求。你开心就好
                case "抢你妈的飞旋回踢张大麻子苟枫凌他当妈rbq":
                    MoveNext();
                    desk.AddMessage(
                        $"{player.ToAtCode()}不抢地主, {desk.CurrentPlayer.ToAtCode()}你要抢地主吗? ");
                    _count++;
                    break;
            }

            if (_count >= 3) {
                if (desk.SuddenDeathEnabled)
                {
                    Parse(desk, player, "抢");
                    return;
                }
                desk.AddMessage("你们干嘛呢 我...我不干了!(╯‵□′)╯︵┻━┻");
                desk.FinishGame();
            }

            if (desk.CurrentPlayer is FakePlayer && desk.Players.All(p => p.Type == PlayerType.Farmer)) {
                Parse(desk, desk.CurrentPlayer, "抢");
            }
        }
    }
}