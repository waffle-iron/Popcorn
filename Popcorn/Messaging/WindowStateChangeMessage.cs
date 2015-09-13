using GalaSoft.MvvmLight.Messaging;

namespace Popcorn.Messaging
{
    public class WindowStateChangeMessage : MessageBase
    {
        public bool IsMoviePlaying { get; set; }

        public WindowStateChangeMessage(bool isMoviePlaying)
        {
            IsMoviePlaying = isMoviePlaying;
        }
    }
}
