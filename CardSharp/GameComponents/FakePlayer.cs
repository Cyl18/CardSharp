using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp.GameComponents
{
    public class FakePlayer : Player
    {
        public FakePlayer(Desk desk) : base(GenerateName(desk))
        {
            HostedEnabled = true;
        }

        private static readonly Random Rng = new Random("fork you kamijoutoma".GetHashCode());

        private static string GenerateName(Desk desk)
        {
            while (true)
            {
                var name = $"机器人{Rng.Next(100)}";
                if (desk.Players.Any(p => p.PlayerId == name)) continue;
                return name;
            }
        }

        public override string ToAtCode()
        {
            return $"{PlayerId}";
        }
    }
}
