using System.Collections.Generic;
using System.Linq;
using CardSharp.GameSteps;

namespace CardSharp
{
    public class LandlordDiscuss : Samsara, ICommandParser
    {
        private readonly IEnumerable<Card> _landlordCards;

        public LandlordDiscuss(IEnumerable<Card> landlordCards, Desk desk)
        {
            _landlordCards = landlordCards;
            desk.AddMessage($"开始游戏, {desk.GetPlayerFromIndex(CurrentIndex).ToAtCode()}你是否要抢地主[抢地主/不抢]");
        }

        public void Parse(Desk desk, Player player, string command)
        {
            if (!IsValidPlayer(desk, player)) return;

            switch (command)
            {
                case "抢地主":
                    player.Cards.AddRange(_landlordCards);
                    player.Cards.Sort();
                    desk.SetLandlord(player);
                    desk.SendCardsMessage();
                    desk.AddMessage($"{player.ToAtCode()}抢地主成功. 为{string.Join("", _landlordCards.Select(card=>$"[{card}]"))}");
                    break;
                case "不抢":
                    MoveNext();
                    desk.AddMessage($"{player.ToAtCode()}不抢地主, {desk.GetPlayerFromIndex(CurrentIndex).ToAtCode()}抢不抢地主");
                    break;
            }
        }
        
    }
}