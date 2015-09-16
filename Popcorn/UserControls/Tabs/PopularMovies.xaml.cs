using System.Windows.Controls;
using Popcorn.ViewModels.Tabs;

namespace Popcorn.UserControls.Tabs
{
    /// <summary>
    /// Interaction logic for PopularMovies.xaml
    /// </summary>
    public partial class PopularMovies
    {
        /// <summary>
        /// Initializes a new instance of the PopularMovies class.
        /// </summary>
        public PopularMovies()
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
            var vm = DataContext as PopularTabViewModel;
            if (vm != null && !vm.IsLoadingMovies)
                await vm.LoadMoviesAsync();
        }
    }
}