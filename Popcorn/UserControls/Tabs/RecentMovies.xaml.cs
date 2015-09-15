using System.Windows.Controls;
using Popcorn.ViewModels.Tabs;

namespace Popcorn.UserControls.Tabs
{
    /// <summary>
    /// Interaction logic for RecentMovies.xaml
    /// </summary>
    public partial class RecentMovies
    {
        /// <summary>
        /// Initializes a new instance of the RecentMovies class.
        /// </summary>
        public RecentMovies()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Decide if we have to load next page regarding to the scroll position
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">ScrollChangedEventArgs</param>
        private async void ScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var totalHeight = e.VerticalOffset + e.ViewportHeight;
            if (!totalHeight.Equals(e.ExtentHeight)) return;
            var vm = DataContext as RecentTabViewModel;
            if (vm != null && !vm.IsLoadingMovies)
                await vm.LoadMoviesAsync();
        }
    }
}