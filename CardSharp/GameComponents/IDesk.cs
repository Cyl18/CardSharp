using System.Collections.Generic;
using CardSharp.Rules;

namespace CardSharp
{
    public interface IDesk
    {
        Player CurrentPlayer { get; }
        IRule CurrentRule { get; set; }
        string DeskId { get; }
        IEnumerable<Card> LastCards { get; }
        Player LastSuccessfulSender { get; }
        string Message { get; }
        List<Player> PlayerList { get; }
        IEnumerable<Player> Players { get; }
        GameState State { get; }

        bool AddPlayer(Player player);
        IEnumerable<Card> GenerateDefaultCards();
        Player GetPlayer(string playerid);
        Player GetPlayerFromIndex(int index);
        void ParseCommand(string playerid, string command);
        void RemovePlayer(Player player);
        void SendCardsMessage();
        bool Start();
    }
}