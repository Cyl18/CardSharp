using Newbe.Mahua.MahuaEvents;
using System;
using Newbe.Mahua;

namespace Origind.Card.Game.MahuaEvents
{
    /// <summary>
    /// 好友申请接受事件
    /// </summary>
    public class FriendAddingRequestMahuaEvent1
        : IFriendAddingRequestMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;

        public FriendAddingRequestMahuaEvent1(
            IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
        }

        public void ProcessAddingFriendRequest(FriendAddingRequestContext context)
        {
            _mahuaApi.AcceptFriendAddingRequest(context.AddingFriendRequestId, context.FromQq, context.FromQq);
            // 不要忘记在MahuaModule中注册
        }
    }
}
