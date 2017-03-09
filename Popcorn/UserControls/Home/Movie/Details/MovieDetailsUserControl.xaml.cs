using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Popcorn.UserControls.Home.Movie.Details
{
    /// <summary>
    /// Interaction logic for Movie.xaml
    /// </summary>
    public partial class MovieDetailsUserControl
    {
        /// <summary>
        /// Initializes a new instance of the Movie class.
        /// </summary>
        public MovieDetailsUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Each time the mouse wheel has been changed, report the event to the parent
        /// </summary>
        /// <param name="sender">The page</param>
        /// <param name="e">MouseWheelEventArgs args</param>
        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = MouseWheelEvent,
                    Source = sender
                };

                var parent = ((Control)sender).Parent as UIElement;
                if (parent == null)
                    return;

                parent.RaiseEvent(eventArg);
            }
        }
    }
}