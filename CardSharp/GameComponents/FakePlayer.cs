using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp.GameComponents
{
    public class FakePlayer : Player
    {
        public FakePlayer() : base(GenerateName())
        {
            HostedEnabled = true;
        }

        private static readonly Random Rng = new Random("fork you kamijoutoma".GetHashCode());
        private static string GenerateName()
        {
            return $"机器人{Rng.Next(100)}";
        }

        public override string ToAtCode()
        {
            return $"{PlayerId}";
        }
    }
}
