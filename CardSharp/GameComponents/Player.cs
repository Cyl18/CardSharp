using System;
using System.Collections.Generic;

namespace CardSharp
{
    public class Player : IEquatable<Player>
    {
        public string PlayerId { get; }

        public Player(string playerId)
        {
            PlayerId = playerId;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Player);
        }

        public bool Equals(Player other)
        {
            return other != null &&
                   PlayerId == other.PlayerId;
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
    }
}
