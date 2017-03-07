using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace Popcorn.Messaging
{
    public class PlayTrailerMessage : MessageBase
    {
        public string TrailerUrl { get; set; }

        public string MovieTitle { get; set; }

        public Action TrailerEndedAction { get; set; }

        public Action TrailerStoppedAction { get; set; }

        public PlayTrailerMessage(string trailerUrl, string movieTitle, Action trailerEndedAction, Action trailerStoppedAction)
        {
            TrailerUrl = trailerUrl;
            MovieTitle = movieTitle;
            TrailerEndedAction = trailerEndedAction;
            TrailerStoppedAction = trailerStoppedAction;
        }
    }
}
