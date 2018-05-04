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
                    if (point <= -100000)
                        desk.AddMessage($"CNM 这里是CY 你们的印钞厂停业了.");
                    else if (point <= 0)
                        desk.AddMessage("您输光了/您没输入过‘获取积分’.");
                    else
                        desk.AddPlayer(player);
                    break;

                case "下桌":
                    desk.RemovePlayer(player);
                    break;

                case "开始游戏":
                    if (desk.Players.All(p => p is FakePlayer))
                    {
                        desk.AddMessage("仨机器人可不行哟~");
                        return;
                    }

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
                        desk.RemovePlayer(desk.Players.First(p => p is FakePlayer));
                    break;
            }

            if (command.StartsWith("开始游戏 ") && PlayerConfig.GetConfig(player).IsAdmin)
            {
                var seed = int.Parse(command.Substring(5));
                if (desk.PlayerList.Count == 3)
                    desk.Start(seed);
            }
            if (command.StartsWith("表演开屎") && PlayerConfig.GetConfig(player).IsAdmin){
                if (desk.Players.Any()){
                    desk.AddMessage("有玩家了不可以使用.");
                    return;
                }

                for(int i = 0; i < 3; i++) desk.AddPlayer(new FakePlayer(desk));
                desk.Start();
            }
        }
    }
}
