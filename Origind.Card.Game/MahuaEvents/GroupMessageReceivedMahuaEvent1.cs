using Newbe.Mahua.MahuaEvents;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CardSharp;
using CardSharp.GameComponents;
using Newbe.Mahua;

namespace Origind.Card.Game.MahuaEvents
{
    /// <summary>
    /// 群消息接收事件
    /// </summary>
    public class GroupMessageReceivedMahuaEvent1
        : IGroupMessageReceivedMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;

        public GroupMessageReceivedMahuaEvent1(
            IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ProcessGroupMessage(GroupMessageReceivedContext context)
        {
            var deskid = context.FromGroup;
            var playerid = context.FromQq;
            var message = context.Message;
            var desk = Desk.GetOrCreateDesk(deskid);
            desk.ParseCommand(playerid, message);
            desk.PlayerList.Where(player => player.Message != null && !(player is FakePlayer)).ToList().ForEach(player =>
            {
                _mahuaApi.SendPrivateMessage(player.PlayerId, player.Message);
                player.ClearMessage();
            });

            Player.ForceSendPlayers.Where(player => player.Message != null && player.ForceSend).ToList().ForEach(player => {
                _mahuaApi.SendPrivateMessage(player.PlayerId, player.Message);
                player.ClearMessage();
                player.ForceSend = false;
            });

            if (desk.Message!=null)
            {
                _mahuaApi.SendGroupMessage(deskid, desk.Message);
                desk.ClearMessage();
            }
        }
    }
}
