using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardSharp.GameComponents;
using Humanizer;
using Humanizer.Localisation;

namespace CardSharp.GameSteps
{
    public class StandardParser : ICommandParser
    {
        public void Parse(Desk desk, Player player, string command)
        {
            var pconfig = PlayerConfig.GetConfig(player);
            switch (command)
            {
                case "获取积分":
                    var px = DateTime.Now - pconfig.LastTime;
                    if (px.TotalSeconds.Seconds() > 12.Hours())
                    {
                        pconfig.AddPoint();
                        desk.AddMessage($"领取成功. 你当前积分为{pconfig.Point}");
                    }
                    else
                    {
                        desk.AddMessage($"你现在不能这么做. 你可以在{(12.Hours() - px).Humanize(culture: new CultureInfo("zh-CN"), maxUnit: TimeUnit.Hour)}后领取.");
                    }
                    break;
                case "我的信息":
                    desk.AddMessage($"你的积分为 {pconfig.Point}");
                    break;
            }
        }
    }
}
