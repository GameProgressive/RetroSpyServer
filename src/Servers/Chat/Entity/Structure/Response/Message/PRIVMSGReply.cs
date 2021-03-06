﻿using Chat.Entity.Structure.User;

namespace Chat.Entity.Structure.Response.Message
{
    public class PRIVMSGReply
    {
        public static string BuildPrivMsgReply(ChatUserInfo senderInfo, string targetName, string message)
        {
            return senderInfo.BuildReply(ChatReplyCode.PRIVMSG, $"{targetName}", message);
        }
    }
}
