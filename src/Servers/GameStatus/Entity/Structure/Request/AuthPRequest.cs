﻿using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using System.Collections.Generic;

namespace GameStatus.Entity.Structure.Request
{

    public class AuthPRequest : GSRequestBase

    {
        public AuthMethod RequestType { get; protected set; }
        public uint ProfileID { get; protected set; }
        public string AuthToken { get; protected set; }
        public string Response { get; protected set; }
        public string KeyHash { get; protected set; }
        public string Nick { get; protected set; }

        public AuthPRequest(Dictionary<string, string> request) : base(request)
        {
        }

        public override GSError Parse()
        {
            var flag = base.Parse();
            if (flag != GSError.NoError)
            {
                return flag;
            }
            if (_request.ContainsKey("pid") && _request.ContainsKey("resp"))
            {
                //we parse profileid here
                uint profileID;
                if (!uint.TryParse(_request["pid"], out profileID))
                {
                    return GSError.Parse;
                }
                ProfileID = profileID;
                RequestType = AuthMethod.ProfileIDAuth;
            }
            else if (_request.ContainsKey("authtoken") && _request.ContainsKey("response"))
            {
                AuthToken = _request["authtoken"];
                Response = _request["response"];
                RequestType = AuthMethod.PartnerIDAuth;
            }
            else if (_request.ContainsKey("keyhash") && _request.ContainsKey("nick"))
            {
                RequestType = AuthMethod.CDkeyAuth;
                KeyHash = _request["keyhash"];
                Nick = _request["nick"];
            }
            else
            {
                return GSError.Parse;
            }

            return GSError.NoError;
        }
    }
}
