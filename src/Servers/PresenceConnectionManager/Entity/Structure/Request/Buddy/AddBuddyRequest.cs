﻿using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceConnectionManager.Entity.Structure.Request.Buddy
{
    public class AddBuddyRequest : PCMRequestBase
    {
        public uint FriendProfileID { get; protected set; }
        public string AddReason { get; protected set; }
        public AddBuddyRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPError Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("sesskey") || !_recv.ContainsKey("newprofileid") || !_recv.ContainsKey("reason"))
            {
                return GPError.Parse;
            }

            uint friendPID;
            if (!uint.TryParse(_recv["newprofileid"], out friendPID))
            {
                return GPError.Parse;
            }

            FriendProfileID = friendPID;
            AddReason = _recv["reason"];

            return GPError.NoError;
        }
    }
}
