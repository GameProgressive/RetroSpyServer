﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatCommand.Channel;
using Chat.Entity.Structure.Response.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CommandHandler.ChatChannelCommandHandler
{
    // Sets channel key/values.
    // If user is NULL or "", the keys will be set on the channel.
    // Otherwise, they will be set on the user,
    // Only ops can set channel keys on other users.
    // Set a value to NULL or "" to clear that key.
    public class GETCHANKEYHandler : ChatChannelHandlerBase
    {
        new GETCHANKEYRequest _request;
        string _values;
        public GETCHANKEYHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = (GETCHANKEYRequest)request;
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            _values = _channel.Property.GetChannelValueString(_request.Keys);
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = GETCHANKEYReply.BuildGetChanKeyReply(
            _user, _channel.Property.ChannelName, _request.Cookie, _values);
        }
    }
}
