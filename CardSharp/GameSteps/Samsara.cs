using System.Linq;

namespace CardSharp.GameSteps
{
    public abstract class Samsara
    {
        public int CurrentIndex;

        protected bool IsValidPlayer(Desk desk, Player player)
        {
            if (!desk.Players.Contains(player))
                return false;
            return desk.Players.ToList().FindIndex(p => p == player) == CurrentIndex;
        }

        protected void MoveNext()
        {
            CurrentIndex = (CurrentIndex + 1) % Constants.MaxPlayer;
        }
    }
}