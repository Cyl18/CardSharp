using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
            var count = 0;
            //Parallel.For(0, 2000, (i) => { RunTest(ref count); });
            for (int i = 0; i < 2000; i++)
            {
                RunTest(ref count);
            }
        }
        private static  Random _rng = new Random("fork you kamijoutoma".GetHashCode());
        private static void RunTest(ref int count)
        {
            var sw = Stopwatch.StartNew();
            var desk = Desk.GetOrCreateDesk(_rng.NextDouble().ToString(CultureInfo.InvariantCulture));
            desk.AddPlayer(new Player("Player1"));
            desk.AddPlayer(new Player("Player2"));
            desk.AddPlayer(new Player("Player3"));

            //Task.Run(() => { ShowMessage(desk); });

            //ParseMessage(desk);

            desk.ParseCommand(desk.CurrentPlayer.PlayerId, "开始游戏");
            desk.ParseCommand(desk.CurrentPlayer.PlayerId, "抢地主");
            desk.ParseCommand(desk.CurrentPlayer.PlayerId, "托管");
            desk.ParseCommand(desk.CurrentPlayer.PlayerId, "托管");
            desk.ParseCommand(desk.CurrentPlayer.PlayerId, "托管");

            Console.WriteLine($"Test successful: {count} / 2000, used {sw.ElapsedMilliseconds}ms");

            Interlocked.Increment(ref count);
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
                    ShowMessage(player, $"[{player.PlayerId}]: ");

                //Thread.Sleep(10);
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
