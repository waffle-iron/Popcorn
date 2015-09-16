using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Popcorn.Messaging;

namespace Popcorn.Models.ApplicationState
{
    public class ApplicationState : ObservableObject, IApplicationState
    {
        private bool _isConnectionInError;
        private bool _isFullScreen;
        private bool _isMoviePlaying;

        /// <summary>
        ///     Indicates if a movie is playing
        /// </summary>
        public bool IsMoviePlaying
        {
            get { return _isMoviePlaying; }
            set
            {
                Set(() => IsMoviePlaying, ref _isMoviePlaying, value);
                Messenger.Default.Send(new WindowStateChangeMessage(value));
            }
        }

        /// <summary>
        ///     Specify if a connection error has occured
        /// </summary>
        public bool IsConnectionInError
        {
            get { return _isConnectionInError; }
            set { Set(() => IsConnectionInError, ref _isConnectionInError, value); }
        }

        /// <summary>
        ///     Indicates if application is fullscreen
        /// </summary>
        public bool IsFullScreen
        {
            get { return _isFullScreen; }
            set
            {
                Set(() => IsFullScreen, ref _isFullScreen, value);
                Messenger.Default.Send(new WindowStateChangeMessage(IsMoviePlaying));
            }
        }
    }
}