using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace Popcorn.UserControls.Players
{
    /// <summary>
    /// Interaction logic for MediaPlayer.xaml
    /// </summary>
    public class MediaPlayer : UserControl
    {
        protected bool Disposed;

        /// <summary>
        /// Indicates if a media is playing 
        /// </summary>
        protected bool MediaPlayerIsPlaying { get; set; }

        /// <summary>
        /// Used to update the activity mouse and mouse position.
        /// </summary>
        protected DispatcherTimer ActivityTimer { get; set; }

        protected Point InactiveMousePosition { get; set; } = new Point(0, 0);

        /// <summary>
        /// Indicate if user is manipulating the timeline player
        /// </summary>
        protected bool UserIsDraggingMediaPlayerSlider { get; set; }

        /// <summary>
        /// Timer used for report time on the timeline
        /// </summary>
        protected DispatcherTimer MediaPlayerTimer { get; set; }

        /// <summary>
        /// Report when dragging is used on media player
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">DragStartedEventArgs</param>
        protected void MediaSliderProgressDragStarted(object sender, DragStartedEventArgs e)
        {
            UserIsDraggingMediaPlayerSlider = true;
        }
    }
}