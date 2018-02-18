using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CardSharp.GameComponents;

namespace CardSharp.GUI
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                RunTest();
            }
        }
        private static readonly Random Rng = new Random("fork you kamijoutoma".GetHashCode());
        private static void RunTest()
        {
            var desk = Desk.GetOrCreateDesk(Rng.NextDouble().ToString(CultureInfo.InvariantCulture));
            desk.AddPlayer(new Player("1"));
            desk.AddPlayer(new FakePlayer(desk));
            desk.AddPlayer(new FakePlayer(desk));

            desk.Start();

            Task.Run(() => { ShowMessage(desk); });
            
            ParseMessage(desk);
        }

        private static void ParseMessage(Desk desk)
        {
            while (desk.State != GameState.Unknown)
            {
                var line = Console.ReadLine();
                desk.ParseCommand(desk.CurrentPlayer.PlayerId, line);
                Thread.Sleep(10);
            }
        }

        private static void ShowMessage(Desk desk)
        {
            while (desk.State != GameState.Unknown)
            {
                if (desk.Message != null)
                    ShowMessage(desk, "[Desk]:    ");
                
                foreach (var player in desk.Players.Where(p => !(p is FakePlayer)))
                    ShowMessage(player, $"[{player.PlayerId}]: ");

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
