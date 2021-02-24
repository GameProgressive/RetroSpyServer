﻿using CDKey.Handler.CmdSwitcher;
using Serilog.Events;
using System;
using System.Net;
using System.Text;
using UniSpyLib.Encryption;
using UniSpyLib.Logging;
using UniSpyLib.Network;

namespace CDKey.Network
{
    internal sealed class CDKeyServer : UniSpyUDPServerBase
    {
        public CDKeyServer(Guid serverID, IPEndPoint endpoint) : base(serverID, endpoint)
        {
        }
        protected override UniSpyUDPSessionBase CreateSession(EndPoint endPoint) => new CDKeySession(this, endPoint);
        protected override byte[] Decryption(byte[] buffer)
        { 
            return XorEncoding.Encrypt(buffer, XorEncoding.XorType.Type0);
        }
        protected override byte[] Encrypt(byte[] buffer)
        {
            return XorEncoding.Encrypt(buffer, XorEncoding.XorType.Type0); ;
        }
        protected override void OnReceived(UniSpyUDPSessionBase session, string message)
            => new CDKeyCmdSwitcher(session, message).Switch();
    }
}
