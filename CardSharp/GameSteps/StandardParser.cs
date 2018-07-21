using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using CardSharp.GameComponents;
using Humanizer;
using Humanizer.Localisation;

namespace CardSharp.GameSteps
{
    public class StandardParser : ICommandParser
    {
        private static readonly Random Random = new Random();
        private static readonly string RandomBotId = Random.Next(1000000).ToString();

        public void Parse(Desk desk, Player player, string command)
        {
            if (command.Contains("当前玩家有: ") && !command.Contains("UNO"))
            {
                desk.AddMessage($"我们目前检测到了一些小小的\"机器人冲突\". 输入[关闭机器人{RandomBotId}]来降低这个机器人在此群的地位.");
            }

            if (command == $"关闭机器人{RandomBotId}")
            {
                Desk.ShutedGroups.Add(desk.DeskId);
                desk.AddMessage($"已经关闭斗地主. 重新恢复为[恢复机器人{RandomBotId}]");
            }
            else if (command == $"恢复机器人{RandomBotId}")
            {
                Desk.ShutedGroups.RemoveAll(d => d == desk.DeskId);
                desk.AddMessage("已经重启斗地主.");
            }

            var pconfig = PlayerConfig.GetConfig(player);
            switch (command)
            {
                case "所有游戏":
                    desk.BoardcastDesks();
                    break;

                case "获取积分":
                case "领取积分":
                    var px = DateTime.Now - pconfig.LastTime;
                    if (px.TotalSeconds.Seconds() > 12.Hours())
                    {
                        if (pconfig.Point > 10000)
                        {
                            desk.AddMessage($"你*没有权限*领取低保资金.");
                        }
                        else
                        {
                            pconfig.AddPoint();
                            desk.AddMessage($"领取成功. 你当前积分有{pconfig.Point}");
                        }
                    }
                    else
                    {
                        desk.AddMessage(
                            $"你现在不能这么做. 你可以在{(12.Hours() - px).Humanize(int.MaxValue,new CultureInfo("zh-CN"), maxUnit: TimeUnit.Hour, minUnit: TimeUnit.Millisecond)}后领取低保资金.");
                    }

                    break;

                case "我的信息":
                    desk.AddMessage($"你有 {pconfig.Point}的积分.");
                    break;

                case "重新发牌":
                    if (desk.State == GameState.Gaming || desk.State == GameState.DiscussLandlord)
                        desk.SendCardsMessage();
                    break;

                case "命令列表":
                    desk.AddMessage(@"=    命令列表    =

/////没牌的直接添加机器人好友(有牌的最好也添加)机器人会自动同意请求\\\\\
Powered by Cy.
Unpowered by LG.
Repowered by Cy.
命令说明：
         带有[D]的命令 还未开发完成
         带有[B]的命令 是测试功能，可能会更改
         带有[R]的命令 是正式功能，'一般'不会做更改
群聊命令：
|上桌|fork table|：加入游戏
|下桌|：退出游戏
|过|不出|不要|出你妈|要你妈|pass|passs|passss|passsss|passsssssssssssssssss|：过牌
|抢地主|抢他妈的|：抢地主
|不抢|抢你妈|抢个鸡毛掸子|：不抢地主
|开始游戏|：准备环节→开始游戏
|重新发牌|:不是重置牌！是会把你的牌通过私聊再发一次！
|加倍|:加倍或超级加倍在一局游戏中只能使用一次
|超级加倍|:加倍再加倍
|减倍|:减倍或超级减倍在一局游戏中只能使用一次
|超级减倍|:减倍再减倍
|SUDDEN_DEATH_DUEL_CARD|: 死亡决斗卡 将会把你所有的积分赌上桌，要么破产，要么暴富
|下桌|：退出游戏，只能在准备环节使用
|明牌|：显示自己的牌给所有玩家，明牌会导致积分翻倍，只能在发完牌后以及出牌之前使用。
|托管|：自动出牌
|结束托管|：结束自动出牌
|弃牌|：放弃本局游戏，当地主或者两名农民弃牌游戏结束,弃牌农民玩家赢了不得分，输了双倍扣分
|结束游戏|：只有参与游戏的人可以使用
|获取积分|：获取积分，12小时可获取10000分。
|我的信息|：你的积分
|记牌器|：显示每种牌在场上还剩下多少张
|安静出牌启用|：所有信息都会私聊发送
|安静出牌禁用|：所有信息不都会私聊发送
|自动过牌启用|：启用自动过牌
|自动过牌禁用|：禁用自动过牌

游戏愉快。
");
                    break;

                case "安静出牌启用":
                    desk.Silence = true;
                    break;

                case "安静出牌禁用":
                    desk.Silence = false;
                    break;

                case "自动过牌启用":
                    player.AutoPass = true;
                    desk.AddMessage("好了.");
                    break;

                case "自动过牌禁用":
                    player.AutoPass = false;
                    desk.AddMessage("好了.");
                    break;

                case "排行榜":
                    ScoreBoard(desk);
                    break;

                case "最近更新":
                    Commits(desk);
                    break;

                case "最后更新":
                    LatestUpdate(desk);
                    break;
            }

            if (pconfig.IsAdmin)
            {
                switch (command)
                {
                    case "结束游戏":
                        desk.FinishGame();
                        break;

                    case "玩家牌":
                        if (!Player.ForceSendPlayers.Contains(player))
                        {
                            Player.ForceSendPlayers.Add(player);
                        }
                        player.ForceSend = true;
                        player.AddMessage(string.Join(Environment.NewLine, desk.Players.Select(p => $"{p.PlayerId} {p.Cards.ToFormatString()}")));
                        break;

                    case "更新CardSharp":
                        Update(desk);
                        break;
                }

                if (command.StartsWith("设置积分"))
                {
                    var sp = command.Split(" ");
                    var target = sp[1];
                    var point = long.Parse(sp[2]);
                    var cfg = PlayerConfig.GetConfig(new Player(target));
                    cfg.Point = point;
                    desk.AddMessage("好了.");
                    cfg.Save();
                }

                if (command.StartsWith("Sudo"))
                {
                    var sp = command.Split(' ');
                    var target = sp[1];
                    var cmd = sp[2];
                    desk.AddMessageLine("[Begin sudo execution block]");
                    desk.ParseCommand(target, cmd);
                    desk.AddMessage(Environment.NewLine + "[End sudo execution block]");
                }
            }
        }

        private void LatestUpdate(Desk desk)
        {
            var data = ReleaseGetter.Get();
            desk.AddMessage($"更新时间为: {data.assets.First().updated_at}");
        }

        private void Update(Desk desk)
        {
            const string path = "Origind.Card.Game\\CardSharp.dll";
            const string filename = "CardSharp.dll";
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                desk.AddMessage($"删除文件时发生错误 {e}");
                return;
            }
            var data = ReleaseGetter.Get();
            var wc = new WebClient();
            wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36");
            wc.DownloadFile(data.assets.First(asset => asset.name == filename).browser_download_url, path);
            desk.AddMessage($"文件下载完成.");
        }

        private static void Commits(Desk desk)
        {
            var updates = CommitsGetter.Get()
                .Take(6);
            var sb2 = new StringBuilder();
            sb2.AppendLine("最近的6次更新: ");
            foreach (var data in updates)
                sb2.AppendLine($"{data.commit.author.date}:{data.commit.author.name} {data.commit.message}");

            desk.AddMessage(sb2.ToString());
        }

        private static void ScoreBoard(Desk desk)
        {
            var configs = Directory.GetFiles(Constants.ConfigDir)
                .Select(File.ReadAllText)
                .Select(PlayerConfig.FromJson)
                .OrderByDescending(conf => conf.Point)
                .Take(10);
            var sb = new StringBuilder();
            sb.AppendLine("积分排行榜: ");
            foreach (var config in configs)
                sb.AppendLine($"{(config.IsAdmin ? "**" : "")}{config.PlayerID}-{config.ToAtCode()}: {config.Point}");

            desk.AddMessage(sb.ToString());
        }
    }
}