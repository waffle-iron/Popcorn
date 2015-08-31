using System.Windows.Controls;
using Popcorn.ViewModels.Tabs;

namespace Popcorn.UserControls.Tabs
{
    /// <summary>
    /// Interaction logic for PopularMovies.xaml
    /// </summary>
    public partial class PopularMovies
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the PopularMovies class.
        /// </summary>
        public PopularMovies()
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
            if (totalHeight.Equals(e.ExtentHeight))
            {
                var vm = DataContext as PopularTabViewModel;
                if (vm != null && !vm.IsLoadingMovies)
                {
                    await vm.LoadMoviesAsync();
                }
            }
        }

        #endregion

        #endregion
    }
}