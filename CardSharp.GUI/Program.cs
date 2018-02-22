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


            while (true) {
                Console.WriteLine("-CardSharp v0 Test Menu-");
                Console.WriteLine();
                Console.WriteLine("1.Play a round");
                Console.WriteLine("2.Run auto test");
                Console.Write("Your choice: ");
                var r = Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine();
                switch (r.Key) {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        RunTest();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        RunAutoTest();
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        SeedGen();
                        break;
                }
                Console.WriteLine();

            }
        }

        private static void SeedGen()
        {
            var cards = Desk.GenerateCards();

            Parallel.For(0, int.MaxValue, i =>
            {
                var list = new List<Card>(cards);
                list.Shuffle(i);

                var pCard1 = list.Take(17).ToListAndSort().ExtractCardGroups();
                var pCard2 = list.Skip(17).Take(17).ToListAndSort().ExtractCardGroups();
                var pCard3 = list.Skip(17*2).Take(17).ToListAndSort().ExtractCardGroups();
                var count = 0;
                foreach (var cardGroup in pCard1)
                {
                    if (cardGroup.Count == 4)
                    {
                        count++;
                    }
                }

                foreach (var cardGroup in pCard2) {
                    if (cardGroup.Count == 4) {
                        count++;
                    }
                }

                foreach (var cardGroup in pCard3) {
                    if (cardGroup.Count == 4) {
                        count++;
                    }
                }

                if (count > 5)
                {
                    Console.WriteLine($"Bomb count: {count}, seed {i} ");
                }
            });
        }

        private static int _count;
        private static void RunAutoTest()
        {
            var sw = Stopwatch.StartNew();
            var desk = Desk.GetOrCreateDesk(Rng.NextDouble().ToString(CultureInfo.InvariantCulture));
            desk.AddPlayer(new FakePlayer(desk));
            desk.AddPlayer(new FakePlayer(desk));
            desk.AddPlayer(new FakePlayer(desk));

            desk.Start();
            _count++;
            Console.WriteLine($"Test successful: {_count}\tUsed {sw.ElapsedMilliseconds}ms.");
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
            while (desk.State != GameState.Unknown) {
                var line = Console.ReadLine();
                desk.ParseCommand(desk.CurrentPlayer.PlayerId, line);
                Thread.Sleep(10);
            }
        }

        private static void ShowMessage(Desk desk)
        {
            while (desk.State != GameState.Unknown) {
                if (desk.Message != null)
                    ShowMessage(desk, "[Desk]:    ");

                foreach (var player in desk.Players.Where(p => !(p is FakePlayer)))
                    ShowMessage(player, $"[{player.PlayerId}]: ");

                Thread.Sleep(10);
            }
        }

        private static void ShowMessage(IMessageSender sender, string id)
        {
            if (sender.Message != null) {
                Console.WriteLine(id + sender.Message);
                sender.ClearMessage();
            }
        }
    }
}
