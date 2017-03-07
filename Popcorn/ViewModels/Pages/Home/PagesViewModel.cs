using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Popcorn.Models.ApplicationState;
using Popcorn.Services.History;
using Popcorn.Services.Movie;
using Popcorn.ViewModels.Pages.Home.Anime;
using Popcorn.ViewModels.Pages.Home.Movie;
using Popcorn.ViewModels.Pages.Home.Movie.Genres;
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

        public PagesViewModel(MoviePageViewModel moviePage)
        {
            moviePage.Caption = "Movies";
            Pages = new ObservableCollection<IPageViewModel>
            {
                moviePage,
                new ShowPageViewModel
                {
                    Caption = "Shows"
                },
                new AnimePageViewModel
                {
                    Caption = "Animes"
                }
            };
        }
    }
}