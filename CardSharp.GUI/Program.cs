using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CardSharp.GUI
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var desk = Desk.GetOrCreateDesk("Test");
                desk.AddPlayer(new Player("Player1"));
                desk.AddPlayer(new Player("Player2"));
                desk.AddPlayer(new Player("Player3"));

                Task.Run(() => { ShowMessage(desk); });

                //ParseMessage(desk);

                desk.ParseCommand(desk.CurrentPlayer.PlayerId, "开始游戏");
                desk.ParseCommand(desk.CurrentPlayer.PlayerId, "抢地主");
                desk.ParseCommand(desk.CurrentPlayer.PlayerId, "托管");
                desk.ParseCommand(desk.CurrentPlayer.PlayerId, "托管");
                desk.ParseCommand(desk.CurrentPlayer.PlayerId, "托管");

                while (desk.State != GameState.Unknown)
                    Thread.Sleep(10);
            }
        }

        private static void ParseMessage(Desk desk)
        {
            while (desk.State != GameState.Unknown)
            {
                var line = Console.ReadLine();
                desk.ParseCommand(desk.CurrentPlayer.PlayerId, line);
            }
        }

        private static void ShowMessage(Desk desk)
        {
            while (desk.State != GameState.Unknown)
            {
                if (desk.Message != null)
                    ShowMessage(desk, "[Desk]:    ");
                
                foreach (var player in desk.Players)
                   // ShowMessage(player, $"[{player.PlayerId}]: ");

                Thread.Sleep(10);
            }
        }

        private static void ShowMessage(IMessageSender sender, string id)
        {
            if (sender.Message!=null)
            {
                Console.WriteLine(id+sender.Message);
                sender.ClearMessage();
            }
        }
    }
}
