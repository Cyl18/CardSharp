using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CardSharp.GameSteps;
using CardSharp.Rules;

namespace CardSharp
{
    public class Desk
    {
        #region Static Members

        private static readonly Dictionary<string, Desk> Desks = new Dictionary<string, Desk>();

        #endregion

        private readonly Dictionary<string, Player> _playersDictionary = new Dictionary<string, Player>();

        private ICommandParser _currentParser;

        public Desk(string deskId)
        {
            DeskId = deskId;
            _currentParser = new WaitingParser();
        }

        public GameState State
        {
            get
            {
                switch (_currentParser)
                {
                    case WaitingParser _:
                        return GameState.Wait;
                    case LandlordDiscuss _:
                        return GameState.DiscussLandlord;
                    case CommandParser _:
                        return GameState.StartGame;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public IEnumerable<Player> Players => _playersDictionary.Values;
        public List<Player> PlayerList => Players.ToList();
        public string DeskId { get; }
        public Player LastSuccessfulSender { get; set; }
        public IEnumerable<Card> LastCards { get; set; }
        public IRule CurrentRule { get; set; }
        public Player CurrentPlayer => GetPlayerFromIndex(((Samsara) _currentParser).CurrentIndex);

        public Player Landlord { get; private set; }

        public IEnumerable<Card> GenerateDefaultCards()
        {
            var list = new List<Card>();
            for (var i = 0; i < Constants.AmountCardNum; i++)
            for (var num = 0; num < Constants.AmountCardMax; num++)
                list.Add(new Card(num));
            list.Add(new Card(Constants.AmountCardMax)); //鬼
            list.Add(new Card(Constants.AmountCardMax + 1)); //王

            list.Shuffle();
            return list;
        }

        public static Desk GetOrCreateDesk(string deskid)
        {
            if (Desks.ContainsKey(deskid))
                return Desks[deskid];

            var desk = new Desk(deskid);
            Desks.Add(deskid, desk);
            return desk;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool AddPlayer(Player player)
        {
            if (Players.Count() >= Constants.MaxPlayer || Players.Contains(player))
                return false;

            _playersDictionary.Add(player.PlayerId, player);
            return true;
        }

        public void RemovePlayer(Player player)
        {
            _playersDictionary.Remove(player.PlayerId);
        }

        public Player GetPlayer(string playerid)
        {
            return _playersDictionary.ContainsKey(playerid) ? _playersDictionary[playerid] : new Player(playerid);
        }

        public bool Start()
        {
            if (State != GameState.Wait)
                return false;
            if (Players.Count() != Constants.MaxPlayer)
                return false;

            _currentParser = new LandlordDiscuss();
            SendCards();
            return true;
        }

        private void SendCards()
        {
        }

        public Player GetPlayerFromIndex(int index)
        {
            return PlayerList[index];
        }

        public string ParseCommand(string playerid, string command)
        {
            return _currentParser.Parse(this, GetPlayer(playerid), command);
        }

        public void SetLandlord(Player player)
        {
            Landlord = player;
        }
    }

    public enum GameState
    {
        Wait,
        DiscussLandlord,
        StartGame
    }
}