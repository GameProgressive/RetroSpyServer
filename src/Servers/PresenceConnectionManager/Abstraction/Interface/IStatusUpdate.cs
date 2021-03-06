﻿namespace PresenceConnectionManager.Abstraction.Interface
{
    public interface IFriendStatusUpdate
    {
        void SubscribeToStatusChange();

        void UnsubscribeToStatusChange();

        void UpdateFriendStatus();
    }
}
