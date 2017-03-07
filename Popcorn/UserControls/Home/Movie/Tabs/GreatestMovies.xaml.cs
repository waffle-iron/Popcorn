using System.Windows.Controls;
using Popcorn.ViewModels.Pages.Home.Movie.Tabs;

namespace Popcorn.UserControls.Home.Movie.Tabs
{
    /// <summary>
    /// Interaction logic for GreatestMovies.xaml
    /// </summary>
    public partial class GreatestMovies
    {
        /// <summary>
        /// Initializes a new instance of the GreatestMovies class.
        /// </summary>
        public GreatestMovies()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load movies if control has reached bottom position
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private async void ScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var totalHeight = e.VerticalOffset + e.ViewportHeight;
            if (!totalHeight.Equals(e.ExtentHeight)) return;
            var vm = DataContext as GreatestTabViewModel;
            if (vm != null && !vm.IsLoadingMovies)
                await vm.LoadMoviesAsync();
        }
    }
}