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
            desk.AddMessage($"开始游戏, {desk.GetPlayerFromIndex(CurrentIndex).ToAtCode()}你要抢地主吗?[抢地主/不抢]");
        }

        public void Parse(Desk desk, Player player, string command)
        {
            if (!IsValidPlayer(desk, player))
                return;

            if (_count > 4)
            {
                desk.AddMessage("你们干嘛呢 我...我不干了!(╯‵□′)╯︵┻━┻");
                desk.FinishGame();
            }

            switch (command) {
                case "抢":
                case "抢地主":
                case "抢他妈的":
                case "抢这个鸡毛掸子": // 应irol的要求. 开心就好啦.
                    player.Cards.AddRange(_landlordCards);
                    player.Cards.Sort();
                    desk.SetLandlord(player);
                    desk.SendCardsMessage();
                    desk.AddMessage($"{player.ToAtCode()}抢地主成功. 为{string.Join("", _landlordCards.Select(card => $"[{card}]"))}");
                    break;
                case "不抢":
                case "抢你妈":
                case "抢个鸡毛掸子": // 应LG的要求。你开心就好
                case "抢你妈的飞旋回踢张大麻子苟枫凌他当妈rbq":
                    MoveNext();
                    desk.AddMessage($"{player.ToAtCode()}不抢地主, {desk.GetPlayerFromIndex(CurrentIndex).ToAtCode()}你要抢地主嘛?");
                    _count++;
                    break;
            }
        }

    }
}