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
                case "fork table":
                case "法克忒薄": // By Charlie Jiang
                    var point = PlayerConfig.GetConfig(player).Point;
                    if (point <= 0)
                        desk.AddMessage($"你的积分不足以进行游戏! 你现在有{point}点积分.");
                    else
                        desk.AddPlayer(player);
                    break;
                case "下桌":
                    desk.RemovePlayer(player);
                    break;
                case "开始游戏":
                    if (desk.PlayerList.Count == 3)
                        desk.Start();
                    else
                        desk.AddMessage("人数不够.");
                    break;
                case "添加机器人":
                    desk.AddPlayer(new FakePlayer(desk));
                    break;
                case "移除机器人":
                    if (desk.Players.Any(p => p is FakePlayer))
                        desk.RemovePlayer(desk.Players.First(p=> p is FakePlayer));
                    break;
            }
        }
    }
}