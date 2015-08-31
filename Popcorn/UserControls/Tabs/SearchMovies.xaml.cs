using System.Windows.Controls;
using Popcorn.ViewModels.Tabs;

namespace Popcorn.UserControls.Tabs
{
    /// <summary>
    /// Interaction logic for SearchMovies.xaml
    /// </summary>
    public partial class SearchMovies
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SearchMovies class.
        /// </summary>
        public SearchMovies()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #region Method -> ScrollViewerScrollChanged

        /// <summary>
        /// Decide if we have to load next page regarding to the scroll position
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">ScrollChangedEventArgs</param>
        private async void ScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var totalHeight = e.VerticalOffset + e.ViewportHeight;
            if (!totalHeight.Equals(e.ExtentHeight)) return;
            var vm = DataContext as SearchTabViewModel;
            if (vm != null && !vm.IsLoadingMovies)
            {
                await vm.SearchMoviesAsync(vm.SearchFilter);
            }
        }

        #endregion

        #endregion
    }
}