using System;
using System.Collections.Generic;

namespace CardSharp
{
    public class Player : IEquatable<Player>, IMessageSender
    {
        public override int GetHashCode()
        {
            return PlayerId.GetHashCode();
        }

        public Player(string playerId)
        {
            PlayerId = playerId;
        }

        public string PlayerId { get; }
        public string Message { get; private set; }
        public List<Card> Cards { get; internal set; }

        public bool Equals(Player other)
        {
            return other != null &&
                   PlayerId == other.PlayerId;
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

        public string ToAtCode()
        {
            return $"[CQ:at,qq={PlayerId}]";
        }

        public void AddMessage(string msg)
        {
            Message += msg;
        }

        public void ClearMessage()
        {
            Message = null;
        }

        public void SendCards(Desk desk)
        {
            AddMessage($"{desk.DeskId} {Cards.ToFormatString()}");
        }
    }
}