namespace CardSharp.GameSteps
{
    internal class WaitingParser : Samsara, ICommandParser
    {
        public void Parse(Desk desk, Player player, string command)
        {
            switch (command)
            {
                case "上桌":
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
            }
            
        }
    }
}