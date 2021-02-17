﻿using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Network;
using Serilog.Events;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace PresenceConnectionManager.Handler.CmdHandler.General
{
    internal class SDKRevisionHandler : PCMCmdHandlerBase
    {
        public SDKRevisionHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            //if (_session.UserInfo.SDKRevision == 0)
            //{
            //    LogWriter.ToLog(LogEventLevel.Error, "[SDKRev] No sdkrevision!");
            //    return;
            //}

            //if ((_session.UserInfo.SDKRevision ^ SDKRevisionType.GPINewAuthNotification) != 0)
            //{
            //    //Send add friend request
            //}

            //if ((_session.UserInfo.SDKRevision ^ SDKRevisionType.GPINewRevokeNotification) != 0)
            //{
            //    //send revoke request
            //}

            //if ((_session.UserInfo.SDKRevision ^ SDKRevisionType.GPINewStatusNotification) != 0)
            //{
            //    //send new status info
            //}

            //if ((_session.UserInfo.SDKRevision ^ SDKRevisionType.GPINewListRetrevalOnLogin) != 0)
            //{
            //    //send buddy list and block list
            //    new BuddyListHandler(_session, null).Handle();
            //    new BlockListHandler(_session, null).Handle();
            //}

            //if ((_session.UserInfo.SDKRevision ^ SDKRevisionType.GPIRemoteAuthIDSNotification) != 0)
            //{
            //    //Remote auth
            //}

            //if ((_session.UserInfo.SDKRevision ^ SDKRevisionType.GPINewCDKeyRegistration) != 0)
            //{
            //    //register cdkey with product id
            //}
        }
    }
}