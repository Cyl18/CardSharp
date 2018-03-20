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
    public class Desk : MessageSenderBase, IDesk, IEquatable<Desk>
    {
        #region Static Members

        private static readonly Dictionary<string, Desk> Desks = new Dictionary<string, Desk>();
        internal static readonly List<string> ShutedGroups = new List<string>();

        #endregion

        private Dictionary<string, Player> _playersDictionary = new Dictionary<string, Player>();

        private readonly StandardParser _standardParser;

        private ICommandParser _currentParser;

        public Desk(string deskId, string groupName = "DefaultGroupName")
        {
            DeskId = deskId;
            GroupName = groupName;
            _currentParser = new WaitingParser();
            _standardParser = new StandardParser();
        }

        public int Multiplier { get; internal set; }

        public bool SuddenDeathEnabled { get; internal set; }

        public bool Silence { get; internal set; }

        public GameState State
        {
            get
            {
                if (!Desks.ContainsValue(this))
                    return GameState.Unknown;

                switch (_currentParser) {
                    case WaitingParser _:
                        return GameState.Wait;
                    case LandlordDiscuss _:
                        return GameState.DiscussLandlord;
                    case CommandParser _:
                        return GameState.Gaming;
                }

                return GameState.Unknown;
            }
        }

        public IEnumerable<Player> Players => _playersDictionary.Values;

        public List<Player> PlayerList => Players.ToList();

        public string DeskId { get; }
        public string GroupName { get; }

        public Player LastSuccessfulSender { get; internal set; }

        public IEnumerable<Card> LastCards { get; internal set; }

        public IRule CurrentRule { get; set; }

        public Player CurrentPlayer => GetPlayerFromIndex(((Samsara)_currentParser).CurrentIndex);

        public IEnumerable<Card> GeneratePlayCards()
        {
            var list = GenerateCards();
            list.Shuffle();
            return list;
        }

        public IEnumerable<Card> GeneratePlayCards(int seed)
        {
            var list = GenerateCards();
            list.Shuffle(seed);
            return list;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool AddPlayer(Player player)
        {
            if (Players.Count() >= Constants.MaxPlayer || Players.Contains(player)) {
                AddMessage($"已经加入或人数已满: {player.ToAtCode()}");
                return false;
            }

            _playersDictionary.Add(player.PlayerId, player);
            AddMessageLine($"加入成功: {player.ToAtCode()}");
            AddMessage($"当前玩家有: {string.Join(", ", Players.Select(p => p.ToAtCode()))}");
            return true;
        }

        public void RemovePlayer(Player player)
        {
            AddMessageLine($"移除成功: {player.ToAtCode()}");
            _playersDictionary.Remove(player.PlayerId);
            AddMessage($"当前玩家有: {string.Join(", ", Players.Select(p => p.ToAtCode()))}");
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

            RandomizePlayers();
            SendCards();
            SendCardsMessage();
            AddMessage("现在可以使用 [加倍/超级加倍/明牌] 之类的命令.");
            return true;
        }

        public bool Start(int seed)
        {
            if (State != GameState.Wait)
                return false;
            if (Players.Count() != Constants.MaxPlayer)
                return false;
            
            SendCards(seed);
            SendCardsMessage();
            AddMessage("现在可以使用 [加倍/超级加倍/明牌] 之类的命令.");
            return true;
        }

        public void SendCardsMessage()
        {
            PlayerList.ForEach(player => player.SendCards(this));
        }

        public Player GetPlayerFromIndex(int index)
        {
            return PlayerList[index];
        }

        public void ParseCommand(string playerid, string command)
        {
            try {
                var player = GetPlayer(playerid);
                if (ShutedGroups.All(g => g != DeskId)) {
                    _currentParser.Parse(this, player, command);
                }
                _standardParser.Parse(this, player, command);
            } catch (Exception e) {
                AddMessage($"抱歉 我们在处理你的命令时发生了错误{e}");
            }
        }

        public bool Equals(Desk other)
        {
            return other != null &&
                   DeskId == other.DeskId;
        }

        public override int GetHashCode()
        {
            return DeskId.GetHashCode();
        }

        public static List<Card> GenerateCards()
        {
            var list = new List<Card>();
            for (var i1 = 0; i1 < 1; i1++) {
                for (var i = 0; i < Constants.AmountCardNum; i++)
                    for (var num = 0; num < Constants.AmountCardMax; num++)
                        list.Add(new Card(num));
                list.Add(new Card(Constants.AmountCardMax)); //鬼
                list.Add(new Card(Constants.AmountCardMax + 1)); //王
            }

            return list;
        }

        public static Desk GetOrCreateDesk(string deskid, string groupName = "DefaultGroupName")
        {
            var gname = groupName ?? "DefaultGroupName";
            if (Desks.ContainsKey(deskid))
                return Desks[deskid];

            var desk = new Desk(deskid, gname);
            Desks.Add(deskid, desk);
            return desk;
        }

        public override void AddMessage(string msg)
        {
            if (Silence)
                SendToAllPlayers(msg);
            else
                base.AddMessage(msg);
        }

        public void SendCards()
        {
            var cards = GeneratePlayCards();
            foreach (var player in Players) {
                var pCards = cards.Take(17 * 1);
                if (player.Cards == null)
                    player.Cards = pCards.ToListAndSort();
                else
                    player.Cards.AddRange(pCards);
                cards = cards.Skip(17 * 1);
            }

            var landlordDiscuss = new LandlordDiscuss(cards, this);
            _currentParser = landlordDiscuss;
            landlordDiscuss.Prepare(this);
        }

        private void SendCards(int seed)
        {
            var cards = GeneratePlayCards(seed);
            foreach (var player in Players) {
                var pCards = cards.Take(17 * 1);
                if (player.Cards == null)
                    player.Cards = pCards.ToListAndSort();
                else
                    player.Cards.AddRange(pCards);
                cards = cards.Skip(17 * 1);
            }

            var landlordDiscuss = new LandlordDiscuss(cards, this);
            _currentParser = landlordDiscuss;
            landlordDiscuss.Prepare(this);
        }

        public void SetLandlord(Player player)
        {
            player.Type = PlayerType.Landlord;
            var parser = new CommandParser(this);
            _currentParser = parser;
            parser.Prepare(this);
        }

        public void BoardcastCards()
        {
            if (CurrentRule == null)
                if (CurrentPlayer.FirstBlood) {
                    CurrentPlayer.FirstBlood = false;
                    AddMessage($"{CurrentPlayer.ToAtCodeWithRole()}请开始你的表演");
                } else {
                    AddMessageLine($"{CurrentPlayer.ToAtCodeWithRole()}请出牌");
                } else
                AddMessage($"{CurrentRule.ToString()}-{LastCards.ToFormatString()} {CurrentPlayer.ToAtCodeWithRole()}请出牌");
        }

        // this is the worst code than I ever written
        public void FinishGame(Player player)
        {
            var mult = GameComponents.Multiplier.CalcResult(this);
            var farmerDif = mult;
            var landlordDif = mult * 2;

            var farmers = Players.Where(p => p.Type == PlayerType.Farmer);
            var landlords = Players.Where(p => p.Type == PlayerType.Landlord);

            if (SuddenDeathEnabled)
            {
                AddMessageLine("SDDC duel done.");

                long result = 0;
                switch (player.Type) {
                    case PlayerType.Farmer:
                        AddMessageLine("Winners are farmers.");
                        result = SaveSddc(farmers, landlords);
                        break;
                    case PlayerType.Landlord:
                        AddMessageLine("Winner is the landlord.");
                        result = SaveSddc(landlords, farmers);
                        break;
                }
                AddMessageLine($"SDDC result: {result}.");

            } else {
                switch (player.Type) {
                    case PlayerType.Farmer:
                        AddMessageLine("农民赢了.");
                        landlordDif *= -1;
                        break;
                    case PlayerType.Landlord:
                        AddMessageLine("地主赢了.");
                        farmerDif *= -1;
                        break;
                }
                var sb = new StringBuilder();

                foreach (var landlord in landlords) {
                    sb.AppendLine($"-{landlord.ToAtCode()} {landlordDif}");
                    var playerConfig = PlayerConfig.GetConfig(landlord);
                    SaveScore(playerConfig, playerConfig.Point + landlordDif);
                }

                foreach (var farmer in farmers) {
                    sb.AppendLine($"-{farmer.ToAtCode()} {farmerDif}");
                    var playerConfig = PlayerConfig.GetConfig(farmer);
                    SaveScore(playerConfig, playerConfig.Point + farmerDif);
                }

                AddMessage(sb.ToString());
            }
            FinishGame(true);
        }

        private long SaveSddc(IEnumerable<Player> winners, IEnumerable<Player> losers)
        {
            var winnersConfig = winners.Select(PlayerConfig.GetConfig).ToList();
            var losersConfig = losers.Select(PlayerConfig.GetConfig).ToList();

            var score = losersConfig.Sum(player => player.Point);
            var pscore = score / winners.Count();

            winnersConfig.ForEach(winner => SaveScore(winner, winner.Point + pscore));
            losersConfig.ForEach(loser => SaveScore(loser, 0));

            return score;
        }

        private void SaveScore(PlayerConfig p, long value)
        {
            var playerConf = p;
            playerConf.Point = value;
            playerConf.Save();
        }

        public void FinishGame(bool force = true)
        {
            if (force) {
                AddMessage("游戏结束.");
                Desks.Remove(DeskId);
                return;
            }
            AddMessage(SuddenDeathEnabled ? "你不能结束游戏." : "游戏结束.");
            if (!SuddenDeathEnabled)
                Desks.Remove(DeskId);
        }

        public void RandomizePlayers()
        {
            var players = new List<Player>(PlayerList);
            players.Shuffle();
            _playersDictionary = players.ToDictionary(player => player.PlayerId);
        }

        public void SendToAllPlayers(string message)
        {
            PlayerList.ForEach(player => player.AddMessage(message));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Desk);
        }


        public static bool operator ==(Desk desk1, Desk desk2)
        {
            return EqualityComparer<Desk>.Default.Equals(desk1, desk2);
        }

        public static bool operator !=(Desk desk1, Desk desk2)
        {
            return !(desk1 == desk2);
        }

        public void BoardcastDesks()
        {
            foreach (var pair in Desks.Where(desk => desk.Value.State == GameState.Gaming))
            {
                AddMessageLine($"群{pair.Value.GroupName}-{pair.Key}正在游戏中");
            }
        }
    }


    public enum GameState
    {
        Wait,
        DiscussLandlord,
        Gaming,
        Unknown
    }
}