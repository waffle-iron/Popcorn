using System;
using GalaSoft.MvvmLight.Messaging;

namespace Popcorn.Messaging
{
    public class NewUpdateMessage : MessageBase
    {
        public Action<bool> CallBack { get; }

        public string UpdateDetails { get; }

        public NewUpdateMessage(Action<bool> callback, string updateDetails)
        {
            CallBack = callback;
            UpdateDetails = updateDetails;
        }
    }
}
