using System.Collections.Generic;
using System.Linq;
using CardSharp.GameSteps;

namespace CardSharp
{
    public class LandlordDiscuss : Samsara, ICommandParser
    {
        private readonly IEnumerable<Card> _landlordCards;
        private int _count = 0;
        
        public LandlordDiscuss(IEnumerable<Card> landlordCards, Desk desk)
        {
            _landlordCards = landlordCards;
            desk.AddMessage($"开始游戏, {desk.GetPlayerFromIndex(CurrentIndex).ToAtCode()}你是否要抢地主[抢地主/不抢]");
        }

        public void Parse(Desk desk, Player player, string command)
        {
            if (!IsValidPlayer(desk, player))
                return;

            if (_count > 4)
            {
                desk.AddMessage("你们干嘛呢 CNM 老子不干了");
                desk.FinishGame();
            }

            switch (command) {
                case "抢地主":
                case "抢他妈的":
                    player.Cards.AddRange(_landlordCards);
                    player.Cards.Sort();
                    desk.SetLandlord(player);
                    desk.SendCardsMessage();
                    desk.AddMessage($"{player.ToAtCode()}抢地主成功. 为{string.Join("", _landlordCards.Select(card => $"[{card}]"))}");
                    break;
                case "不抢":
                case "抢你妈":
                case "抢个鸡毛掸子": // 应LG的要求。你开心就好
                    MoveNext();
                    desk.AddMessage($"{player.ToAtCode()}不抢地主, {desk.GetPlayerFromIndex(CurrentIndex).ToAtCode()}抢不抢地主");
                    _count++;
                    break;
            }
        }

    }
}