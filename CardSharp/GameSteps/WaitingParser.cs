using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp.GameSteps
{
    class WaitingParser : Samsara, ICommandParser
    {
        public string Parse(Desk desk, Player player, string command)
        {
            switch (command)
            {
                case "上桌":
                    desk.AddPlayer(player);
                    return "";
                case "下桌":
                    desk.RemovePlayer(player);
                    return "";
                case "开始游戏":
                    desk.Start();
                    return "";
            }

            return null;
        }
    }
}
