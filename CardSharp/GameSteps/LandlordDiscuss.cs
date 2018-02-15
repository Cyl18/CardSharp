using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardSharp.GameSteps;

namespace CardSharp
{
    public class LandlordDiscuss : Samsara, ICommandParser
    {
        public string Prepare(Desk desk)
        {
            return $"开始游戏, {desk.GetPlayerFromIndex(CurrentIndex)}你是否要抢地主[抢地主/不抢]";
        }

        public string Parse(Desk desk, Player player, string command)
        {
            if (!IsValidPlayer(desk, player)) return null;

            switch (command) {
                case "抢地主":
                    desk.SetLandlord(player);
                    return $"{player.ToAtCode()}抢地主成功.";
                case "不抢":
                    MoveNext();
                    return $"{player.ToAtCode()}不抢地主, {desk.GetPlayerFromIndex(CurrentIndex)}抢不抢地主";
            }

            return null;
        }
    }
}
