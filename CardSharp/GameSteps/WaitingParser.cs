using System.Linq;
using CardSharp.GameComponents;

namespace CardSharp.GameSteps
{
    internal class WaitingParser : Samsara, ICommandParser
    {
        public void Parse(Desk desk, Player player, string command)
        {
            switch (command)
            {
                case "上桌":
                case "上喵":
                case "fork table":
                case "法克忒薄": // By Charlie Jiang
                    //var point = PlayerConfig.GetConfig(player).Point;
                    //if (point <= 0)
                    //    desk.AddMessage($"你的积分不足以进行游戏! 你现在有{point}点积分.");
                    //else
                        desk.AddPlayer(player);
                    break;
                case "下桌":
                case "下喵":
                    desk.RemovePlayer(player);
                    break;
                case "开始游戏":
                    if (desk.Players.All(p => p is FakePlayer))
                    {
                        desk.AddMessage("三只机器猫可是不喜欢打架的呢~");
                        return;
                    }
                    if (desk.PlayerList.Count == 3)
                        desk.Start();
                    else
                        desk.AddMessage("没有足够的喵酱哦~再找几只喵酱陪我玩嘛~");
                    break;
                case "添加机器人":
                case "添加机器猫":
                    desk.AddPlayer(new FakePlayer(desk));
                    break;
                case "移除机器人":
                case "移除机器猫":
                    if (desk.Players.Any(p => p is FakePlayer))
                        desk.RemovePlayer(desk.Players.First(p=> p is FakePlayer));
                    break;
            }

            if (command.StartsWith("开始游戏 ") || command.StartsWith("开始吸猫 "))
            {
                var seed = int.Parse(command.Substring(5));
                if (desk.PlayerList.Count == 3)
                    desk.Start(seed);
            }
        }
    }
}