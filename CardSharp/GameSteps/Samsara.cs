using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp.GameSteps
{
    public abstract class Samsara
    {
        public int CurrentIndex = 0;

        protected bool IsValidPlayer(Desk desk, Player player)
        {
            if (!desk.Players.Contains(player))
                return false;
            return desk.Players.ToList().FindIndex(p => p == player) == CurrentIndex;
        }

        protected void MoveNext() => CurrentIndex = (CurrentIndex + 1) % Constants.MaxPlayer;
    }
}
