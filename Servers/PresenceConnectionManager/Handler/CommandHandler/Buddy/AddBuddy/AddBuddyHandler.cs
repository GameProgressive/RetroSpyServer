﻿using PresenceConnectionManager.Enumerator;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Buddy.AddBuddy
{
    //\addbuddy\\sesskey\<>\newprofileid\<>\reason\<>\final\
    public class AddBuddyHandler : CommandHandlerBase
    {
        public AddBuddyHandler() : base()
        {
        }

        private uint _friendPid;

        protected override void CheckRequest(GPCMSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);

            if (!recv.ContainsKey("sesskey") || !recv.ContainsKey("newprofileid") || !recv.ContainsKey("reason"))
            {
                _errorCode = GPErrorCode.Parse;
            }

            if (!uint.TryParse(recv["newprofileid"], out _friendPid))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataOperation(GPCMSession session, Dictionary<string, string> recv)
        {
            //Check if the friend is online
            //if(online)
            //else
            //store add request to database
        }
    }
}
