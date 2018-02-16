using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CardSharp.GameComponents;
using CardSharp.GameSteps;
using CardSharp.Rules;
// ReSharper disable PossibleMultipleEnumeration

namespace CardSharp
{
    public class Desk : IMessageSender, IDesk
    {
        #region Static Members

        private static readonly Dictionary<string, Desk> Desks = new Dictionary<string, Desk>();

        #endregion

        private readonly Dictionary<string, Player> _playersDictionary = new Dictionary<string, Player>();

        private ICommandParser _currentParser;
        private readonly StandardParser _standardParser;

        public Desk(string deskId)
        {
            DeskId = deskId;
            _currentParser = new WaitingParser();
            _standardParser = new StandardParser();
        }

        public GameState State
        {
            get
            {
                switch (_currentParser) {
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
        public Player LastSuccessfulSender { get; internal set; }
        public IEnumerable<Card> LastCards { get; internal set; }
        public IRule CurrentRule { get; set; }
        public Player CurrentPlayer => GetPlayerFromIndex(((Samsara)_currentParser).CurrentIndex);
        public string Message { get; private set; }
        public int Multiplier { get; internal set; }

        public IEnumerable<Card> GenerateDefaultCards()
        {
            var list = new List<Card>();
            for (var i1 = 0; i1 < 1; i1++)
            {
                for (var i = 0; i < Constants.AmountCardNum; i++)
                for (var num = 0; num < Constants.AmountCardMax; num++)
                    list.Add(new Card(num));
                list.Add(new Card(Constants.AmountCardMax)); //鬼
                list.Add(new Card(Constants.AmountCardMax + 1)); //王
            }

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
            if (Players.Count() >= Constants.MaxPlayer || Players.Contains(player)) {
                AddMessage($"已经加入或人数已满: {player.ToAtCode()}");
                return false;
            }

            _playersDictionary.Add(player.PlayerId, player);
            AddMessage($"加入成功: {player.ToAtCode()}");
            return true;
        }

        public void RemovePlayer(Player player)
        {
            AddMessage($"移除成功: {player.ToAtCode()}");
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

            SendCards();
            SendCardsMessage();
            return true;
        }

        public void SendCardsMessage()
        {
            PlayerList.ForEach(player => player.SendCards(this));
        }

        private void SendCards()
        {
            var cards = GenerateDefaultCards();
            foreach (var player in Players) {
                var pCards = cards.Take(17 * 1);
                player.Cards = pCards.ToListAndSort();
                cards = cards.Skip(17 * 1);
            }
            _currentParser = new LandlordDiscuss(cards, this);
        }

        public Player GetPlayerFromIndex(int index)
        {
            return PlayerList[index];
        }

        public void ParseCommand(string playerid, string command)
        {
            var player = GetPlayer(playerid);
            _currentParser.Parse(this, player, command);
            _standardParser.Parse(this, player, command);
        }

        public void SetLandlord(Player player)
        {
            player.Type = PlayerType.Landlord;
            this._currentParser = new CommandParser(this);
        }

        public void AddMessage(string msg)
        {
            Message += msg;
        }

        public void ClearMessage()
        {
            Message = null;
        }



        public void BoardcastCards()
        {
            AddMessage(CurrentRule == null
                ? $"{CurrentPlayer.ToAtCode()}请出牌"
                : $"{CurrentRule.ToString()}-{string.Join(string.Empty, LastCards.Select(card => $"[{card}]"))} {CurrentPlayer.ToAtCode()}请出牌");
        }

        public void FinishGame(Player player)
        {
            var mult = GameComponents.Multiplier.CalcResult(this);
            var farmerDif = mult;
            var landlordDif = mult * 2;
            switch (player.Type) {
                case PlayerType.Farmer:
                    AddMessage("农民赢了.");
                    landlordDif *= -1;
                    break;
                case PlayerType.Landlord:
                    AddMessage("地主赢了.");
                    farmerDif *= -1;
                    break;
            }

            var sb = new StringBuilder();
            var farmers = Players.Where(p => p.Type == PlayerType.Farmer);
            var landlords = Players.Where(p => p.Type == PlayerType.Landlord);
            foreach (var landlord in landlords) {
                sb.AppendLine($"-{landlord.ToAtCode()} {landlordDif}");
                SaveScore(landlord, landlordDif);
            }

            foreach (var farmer in farmers) {
                sb.AppendLine($"-{farmer.ToAtCode()} {farmerDif}");
                SaveScore(farmer, farmerDif);
            }
            
            AddMessage(sb.ToString());
            FinishGame();

            void SaveScore(Player p, int dif)
            {
                var playerConf = PlayerConfig.GetConfig(p);
                playerConf.Point += dif;
                playerConf.Save();
            }
        }

        public void FinishGame()
        {
            AddMessage("游戏结束.");
            Desks.Remove(this.DeskId);
        }
    }


    public enum GameState
    {
        Wait,
        DiscussLandlord,
        StartGame
    }
}