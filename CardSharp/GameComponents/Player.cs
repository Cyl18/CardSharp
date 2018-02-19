using System;
using System.Collections.Generic;
using System.Linq;

namespace CardSharp
{
    public class Player : MessageSenderBase, IEquatable<Player>
    {
        public Player(string playerId)
        {
            PlayerId = playerId;
            
        }
        
        public static HashSet<Player> ForceSendPlayers => new HashSet<Player>();

        public bool AutoPass { get; set; }
        public bool ForceSend { get; set; }
        public string PlayerId { get; }
        public List<Card> Cards { get; internal set; }
        public PlayerType Type { get; internal set; } = PlayerType.Farmer;
        public bool GiveUp { get; internal set; }
        public bool FirstBlood { get; internal set; } = true;
        public bool Multiplied { get; internal set; }
        public bool PublicCards { get; internal set; }
        public bool HostedEnabled { get; internal set; }

        public bool Equals(Player other)
        {
            return other != null &&
                   PlayerId == other.PlayerId;
        }

        public override int GetHashCode()
        {
            return PlayerId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Player);
        }

        public static bool operator ==(Player player1, Player player2)
        {
            return EqualityComparer<Player>.Default.Equals(player1, player2);
        }

        public static bool operator !=(Player player1, Player player2)
        {
            return !(player1 == player2);
        }

        public virtual string ToAtCode()
        {
#if !DEBUG
            return $"[CQ:at,qq={PlayerId}]";
#else
            return $"{PlayerId}";
#endif
        }

        public virtual string ToAtCodeWithRole()
        {
            return $"{RoleToString()}[CQ:at,qq={PlayerId}]";
        }

        protected string RoleToString()
        {
            switch (Type)
            {
                case PlayerType.Farmer:
                    return "农民";
                case PlayerType.Landlord:
                    return "地主";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SendCards(Desk desk)
        {
            AddMessage($"{desk.DeskId} {Cards.ToFormatString()}");
        }
    }

    public enum PlayerType
    {
        Farmer,
        Landlord
    }
}