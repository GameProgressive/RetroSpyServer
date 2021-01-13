﻿using UniSpyLib.Abstraction.Interface;
using System.Collections.Generic;
using PresenceConnectionManager.Network;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal class LogoutHandler : PCMCmdHandlerBase
    {
        public LogoutHandler(IUniSpySession session,IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            _session.Disconnect();
            PCMServer.LoggedInSession.Remove(_session.Id, out _);
        }
    }
}