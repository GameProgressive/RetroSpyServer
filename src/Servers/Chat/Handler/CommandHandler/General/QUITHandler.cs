﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.SystemHandler.ChatSessionManage;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class QUITHandler : ChatLogedInHandlerBase
    {
        new QUITRequest _request;
        public QUITHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (QUITRequest)request;
        }

        protected override void DataOperation()
        {
            base.DataOperation();

            foreach (var channel in _session.UserInfo.JoinedChannels)
            {
                channel.LeaveChannel(_session, _request.Reason);
            }
            ChatSessionManager.RemoveSession(_session);
        }
    }
}
