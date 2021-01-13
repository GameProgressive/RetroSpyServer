﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using NATNegotiation.Application;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Entity.Structure.Response;
using NATNegotiation.Entity.Structure.Result;
using Serilog.Events;
using UniSpyLib.Logging;
using System;
namespace NATNegotiation.Handler.SystemHandler.Manager
{
    public class NatNegotiateManager
    {
        public static void Negotiate(NatPortType portType, byte version, uint cookie)
        {
            var searchKey = NatUserInfo.RedisOperator.BuildSearchKey(portType, cookie);

            List<string> matchedKeys = NatUserInfo.RedisOperator.GetMatchedKeys(searchKey);
            // because cookie is unique for each client we will only get 2 of keys
            if (matchedKeys.Count != 2)
            {
                LogWriter.ToLog("No match found we continue waitting.");
                return;
            }

            Dictionary<string, NatUserInfo> negotiatorPairs = new Dictionary<string, NatUserInfo>();

            foreach (var key in matchedKeys)
            {
                negotiatorPairs.Add(key, NatUserInfo.RedisOperator.GetSpecificValue(key));
            }

            ////find negitiators and negotiatees by a same cookie
            var negotiators = negotiatorPairs.Where(s => s.Value.ClientIndex == 0);
            var negotiatees = negotiatorPairs.Where(s => s.Value.ClientIndex == 1);

            if (negotiators.Count() != 1 || negotiatees.Count() != 1)
            {
                LogWriter.ToLog("No match found we continue waitting!");
                return;
            }

            // we only can find one pair of the users
            var negotiator = negotiators.First();
            var negotiatee = negotiatees.First();

            LogWriter.ToLog(LogEventLevel.Debug, $"Find negotiator {negotiator.Value.RemoteEndPoint}");
            LogWriter.ToLog(LogEventLevel.Debug, $"Find negotiatee {negotiatee.Value.RemoteEndPoint}");
            // exchange data for each other
            EndPoint endOfNegotiator = IPEndPoint.Parse(negotiator.Value.RemoteEndPoint);
            EndPoint endOfNegotiatee = IPEndPoint.Parse(negotiatee.Value.RemoteEndPoint);

            var result1 = new ConnectResult();
            result1.RemoteEndPoint = endOfNegotiatee;
            result1.Cookie = cookie;
            var result2 = new ConnectResult();
            result2.RemoteEndPoint = endOfNegotiator;

            //TODO
            throw new NotImplementedException();
            //byte[] dataToNegotiator =
            //    new ConnectResponse(version, cookie, endOfNegotiatee);
            //byte[] dataToNegotiatee =
            //    new ConnectResponse(version, cookie, endOfNegotiator);

            //NNServerManager.Server.SendAsync(endOfNegotiator, dataToNegotiator);
            //NNServerManager.Server.SendAsync(endOfNegotiatee, dataToNegotiatee);
        }
    }
}