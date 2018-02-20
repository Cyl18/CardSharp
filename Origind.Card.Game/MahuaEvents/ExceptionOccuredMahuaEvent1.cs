using Newbe.Mahua.MahuaEvents;
using System;
using Newbe.Mahua;

namespace Origind.Card.Game.MahuaEvents
{
    /// <summary>
    /// 运行出现异常事件
    /// </summary>
    public class ExceptionOccuredMahuaEvent1
        : IExceptionOccuredMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;

        public ExceptionOccuredMahuaEvent1(
            IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
        }

        public void HandleException(ExceptionOccuredContext context)
        {
            // todo 填充处理逻辑

            context.ContinueThrows = false;
            // 不要忘记在MahuaModule中注册
        }
    }
}
