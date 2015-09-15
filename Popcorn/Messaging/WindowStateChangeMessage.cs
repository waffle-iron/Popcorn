using GalaSoft.MvvmLight.Messaging;

namespace Popcorn.Messaging
{
    public class WindowStateChangeMessage : MessageBase
    {
        public WindowStateChangeMessage(bool isMoviePlaying)
        {
            IsMoviePlaying = isMoviePlaying;
        }

        public bool IsMoviePlaying { get; set; }
    }
}