﻿using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using Serilog.Events;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Handler.SystemHandler.Error;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionSwitcher
{
    public class SBUpdateOptionSwitcher
    {
        public static void Switch(ISession session, byte[] recv)
        {
            ServerListRequest request = new ServerListRequest();
            if (!request.Parse(recv))
            {
                LogWriter.ToLog(LogEventLevel.Error, ErrorMessage.GetErrorMsg(SBErrorCode.Parse));
                return;
            }
            switch (request.UpdateOption)
            {
                case SBServerListUpdateOption.NoServerList:
                    new NoServerListHandler(request, session, recv).Handle();
                    break;
                case SBServerListUpdateOption.GeneralRequest:
                    new GeneralRequestHandler(request, session, recv).Handle();
                    break;
                case SBServerListUpdateOption.SendGroups:
                    new SendGroupsHandler(request, session, recv).Handle();
                    break;
                case SBServerListUpdateOption.LimitResultCount:
                    break;
                case SBServerListUpdateOption.PushUpdates:
                    // worms 3d send this after join group room
                    // we should send adhoc servers which are in this room to worms3d
                    new PushUpdatesHandler(request, session, recv).Handle();
                    break;
            }
        }
    }
}
