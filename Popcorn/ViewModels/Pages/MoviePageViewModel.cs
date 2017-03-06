using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Popcorn.Events;
using Popcorn.Messaging;
using Popcorn.Models.ApplicationState;
using Popcorn.Services.History;
using Popcorn.Services.Movie;
using Popcorn.ViewModels.Genres;
using Popcorn.ViewModels.Players.Movie;
using Popcorn.ViewModels.Tabs;

namespace Popcorn.ViewModels.Pages
{
    public class MoviePageViewModel : PageViewModel
    {
        /// <summary>
        /// Used to interact with movie history
        /// </summary>
        private readonly IMovieHistoryService _movieHistoryService;

        /// <summary>
        /// Used to interact with movies
        /// </summary>
        private readonly IMovieService _movieService;

        /// <summary>
        /// Application state
        /// </summary>
        private IApplicationState _applicationState;

        /// <summary>
        /// Manage movie's genres
        /// </summary>
        private IGenresViewModel _genresViewModel;

        /// <summary>
        /// Specify if a search is actually active
        /// </summary>
        private bool _isMovieSearchActive;

        /// <summary>
        /// Manage the movie player
        /// </summary>
        private MoviePlayerViewModel _moviePlayerViewModel;

        /// <summary>
        /// The selected tab
        /// </summary>
        private TabsViewModel _selectedTab;

        /// <summary>
        /// The tabs
        /// </summary>
        private ObservableCollection<TabsViewModel> _tabs = new ObservableCollection<TabsViewModel>();

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// <param name="genresViewModel">Instance of GenresViewModel</param>
        /// <param name="movieService">Instance of MovieService</param>
        /// <param name="movieHistoryService">Instance of MovieHistoryService</param>
        /// <param name="applicationState">Instance of ApplicationState</param>
        public MoviePageViewModel(IGenresViewModel genresViewModel, IMovieService movieService,
            IMovieHistoryService movieHistoryService, IApplicationState applicationState)
        {
            _movieService = movieService;
            _movieHistoryService = movieHistoryService;
            ApplicationState = applicationState;
            GenresViewModel = genresViewModel;
            RegisterMessages();
            RegisterCommands();
            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                Tabs.Add(new PopularTabViewModel(ApplicationState, _movieService, _movieHistoryService));
                Tabs.Add(new GreatestTabViewModel(ApplicationState, _movieService, _movieHistoryService));
                Tabs.Add(new RecentTabViewModel(ApplicationState, _movieService, _movieHistoryService));
                Tabs.Add(new FavoritesTabViewModel(ApplicationState, _movieService, _movieHistoryService));
                Tabs.Add(new SeenTabViewModel(ApplicationState, _movieService, _movieHistoryService));
                SelectedTab = Tabs.First();
                foreach (var tab in Tabs)
                    await tab.LoadMoviesAsync();

                await GenresViewModel.LoadGenresAsync();
            });
        }

        /// <summary>
        /// Specify if a movie search is active
        /// </summary>
        public bool IsMovieSearchActive
        {
            get { return _isMovieSearchActive; }
            private set { Set(() => IsMovieSearchActive, ref _isMovieSearchActive, value); }
        }

        /// <summary>
        /// Event raised when window state has changed
        /// </summary>
        public event EventHandler<WindowStateChangedEventArgs> WindowStateChanged;

        /// <summary>
        /// Tabs shown into the interface
        /// </summary>
        public ObservableCollection<TabsViewModel> Tabs
        {
            get { return _tabs; }
            set { Set(() => Tabs, ref _tabs, value); }
        }

        /// <summary>
        /// The selected tab
        /// </summary>
        public TabsViewModel SelectedTab
        {
            get { return _selectedTab; }
            set { Set(() => SelectedTab, ref _selectedTab, value); }
        }

        /// <summary>
        /// Register messages
        /// </summary>
        private void RegisterMessages()
        {
            Messenger.Default.Register<WindowStateChangeMessage>(this,
                e => OnWindowStateChanged(new WindowStateChangedEventArgs(e.IsMoviePlaying)));

            Messenger.Default.Register<PlayMovieMessage>(this, message => DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                _moviePlayerViewModel = new MoviePlayerViewModel(ApplicationState, _movieService,
                    _movieHistoryService);
                _moviePlayerViewModel.LoadMovie(message.Movie);
                Tabs.Add(_moviePlayerViewModel);
                SelectedTab = Tabs.Last();
                ApplicationState.IsMoviePlaying = true;
            }));

            Messenger.Default.Register<StopPlayingMovieMessage>(
                this,
                message =>
                {
                    // Remove the movie tab
                    MoviePlayerViewModel moviePlayer = null;
                    foreach (var mediaViewModel in Tabs.OfType<MoviePlayerViewModel>())
                        moviePlayer = mediaViewModel;

                    if (moviePlayer != null)
                    {
                        Tabs.Remove(moviePlayer);
                        moviePlayer.Cleanup();
                        SelectedTab = Tabs.FirstOrDefault();
                    }

                    ApplicationState.IsMoviePlaying = false;
                });

            Messenger.Default.Register<SearchMovieMessage>(this,
                async message => await SearchMovies(message.Filter));
        }

        /// <summary>
        /// Register commands
        /// </summary>
        private void RegisterCommands()
        {
            SelectGreatestTab = new RelayCommand(() =>
            {
                if (SelectedTab is GreatestTabViewModel)
                    return;
                foreach (var greatestTab in Tabs.OfType<GreatestTabViewModel>())
                    SelectedTab = greatestTab;
            });

            SelectPopularTab = new RelayCommand(() =>
            {
                if (SelectedTab is PopularTabViewModel)
                    return;
                foreach (var popularTab in Tabs.OfType<PopularTabViewModel>())
                    SelectedTab = popularTab;
            });

            SelectRecentTab = new RelayCommand(() =>
            {
                if (SelectedTab is RecentTabViewModel)
                    return;
                foreach (var recentTab in Tabs.OfType<RecentTabViewModel>())
                    SelectedTab = recentTab;
            });

            SelectSearchTab = new RelayCommand(() =>
            {
                if (SelectedTab is SearchTabViewModel)
                    return;
                foreach (var searchTab in Tabs.OfType<SearchTabViewModel>())
                    SelectedTab = searchTab;
            });

            SelectFavoritesTab = new RelayCommand(() =>
            {
                if (SelectedTab is FavoritesTabViewModel)
                    return;
                foreach (var favoritesTab in Tabs.OfType<FavoritesTabViewModel>())
                    SelectedTab = favoritesTab;
            });

            SelectSeenTab = new RelayCommand(() =>
            {
                if (SelectedTab is SeenTabViewModel)
                    return;
                foreach (var seenTab in Tabs.OfType<SeenTabViewModel>())
                    SelectedTab = seenTab;
            });
        }

        /// <summary>
        /// Manage movie's genres
        /// </summary>
        public IGenresViewModel GenresViewModel
        {
            get { return _genresViewModel; }
            set { Set(() => GenresViewModel, ref _genresViewModel, value); }
        }

        /// <summary>
        /// Application state
        /// </summary>
        public IApplicationState ApplicationState
        {
            get { return _applicationState; }
            set { Set(() => ApplicationState, ref _applicationState, value); }
        }

        /// <summary>
        /// Command used to select the greatest movies tab
        /// </summary>
        public RelayCommand SelectGreatestTab { get; private set; }

        /// <summary>
        /// Command used to select the popular movies tab
        /// </summary>
        public RelayCommand SelectPopularTab { get; private set; }

        /// <summary>
        /// Command used to select the recent movies tab
        /// </summary>
        public RelayCommand SelectRecentTab { get; private set; }

        /// <summary>
        /// Command used to select the search movies tab
        /// </summary>
        public RelayCommand SelectSearchTab { get; private set; }

        /// <summary>
        /// Command used to select the seen movies tab
        /// </summary>
        public RelayCommand SelectSeenTab { get; private set; }

        /// <summary>
        /// Command used to select the favorites movies tab
        /// </summary>
        public RelayCommand SelectFavoritesTab { get; private set; }

        /// <summary>
        /// Search for movie with a criteria
        /// </summary>
        /// <param name="criteria">The criteria used for search</param>
        private async Task SearchMovies(string criteria)
        {
            if (string.IsNullOrEmpty(criteria))
            {
                // The search filter is empty. We have to find the search tab if any
                foreach (var searchTabToRemove in Tabs.OfType<SearchTabViewModel>())
                {
                    // The search tab is currently selected in the UI, we have to pick a different selected tab prior deleting
                    if (searchTabToRemove == SelectedTab)
                        SelectedTab = Tabs.FirstOrDefault();

                    Tabs.Remove(searchTabToRemove);
                    searchTabToRemove.Cleanup();
                    IsMovieSearchActive = false;
                    return;
                }
            }
            else
            {
                IsMovieSearchActive = true;

                foreach (var searchTab in Tabs.OfType<SearchTabViewModel>())
                {
                    await searchTab.SearchMoviesAsync(criteria);
                    if (SelectedTab != searchTab)
                        SelectedTab = searchTab;

                    return;
                }

                Tabs.Add(new SearchTabViewModel(ApplicationState, _movieService, _movieHistoryService));
                SelectedTab = Tabs.Last();
                var searchMovieTab = SelectedTab as SearchTabViewModel;
                if (searchMovieTab != null)
                    await searchMovieTab.SearchMoviesAsync(criteria);
            }
        }

        /// <summary>
        /// Fire when window state has changed
        /// </summary>
        /// <param name="e">Event args</param>
        private void OnWindowStateChanged(WindowStateChangedEventArgs e)
        {
            var handler = WindowStateChanged;
            handler?.Invoke(this, e);
        }
    }
}
