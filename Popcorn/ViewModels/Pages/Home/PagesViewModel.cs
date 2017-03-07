using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Popcorn.ViewModels.Pages.Home.Anime;
using Popcorn.ViewModels.Pages.Home.Movie;
using Popcorn.ViewModels.Pages.Home.Show;

namespace Popcorn.ViewModels.Pages.Home
{
    /// <summary>
    /// Represents a page
    /// </summary>
    public class PagesViewModel : ViewModelBase
    {
        /// <summary>
        /// The pages
        /// </summary>
        private ObservableCollection<IPageViewModel> _pages = new ObservableCollection<IPageViewModel>();

        /// <summary>
        /// Pages shown into the interface
        /// </summary>
        public ObservableCollection<IPageViewModel> Pages
        {
            get { return _pages; }
            set { Set(() => Pages, ref _pages, value); }
        }

        /// <summary>
        /// Create an instance of <see cref="PagesViewModel"/>
        /// </summary>
        /// <param name="moviePage">Movie page</param>
        /// <param name="animePage">Anime page</param>
        /// <param name="showPage">Show page</param>
        public PagesViewModel(MoviePageViewModel moviePage, AnimePageViewModel animePage, ShowPageViewModel showPage)
        {
            moviePage.Caption = "Movies";
            animePage.Caption = "Animes";
            showPage.Caption = "Shows";
            Pages = new ObservableCollection<IPageViewModel>
            {
                moviePage,
                showPage,
                animePage
            };
        }
    }
}