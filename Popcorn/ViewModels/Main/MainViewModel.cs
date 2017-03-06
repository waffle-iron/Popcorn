using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using Popcorn.Dialogs;
using Popcorn.Events;
using Popcorn.Helpers;
using Popcorn.Messaging;
using Popcorn.Models.ApplicationState;
using Popcorn.Services.History;
using Popcorn.Services.Movie;
using Popcorn.ViewModels.Genres;
using Popcorn.ViewModels.Pages;
using Squirrel;

namespace Popcorn.ViewModels.Main
{
    /// <summary>
    /// Main applcation's viewmodel
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Used to define the dialog context
        /// </summary>
        private readonly IDialogCoordinator _dialogCoordinator;

        /// <summary>
        /// Specify if an exception is curently managed
        /// </summary>
        private bool _isManagingException;

        /// <summary>
        /// Specify if movie flyout is open
        /// </summary>
        private bool _isMovieFlyoutOpen;

        /// <summary>
        /// Specify if settings flyout is open
        /// </summary>
        private bool _isSettingsFlyoutOpen;

        /// <summary>
        /// The pages
        /// </summary>
        private ObservableCollection<PageViewModel> _pages = new ObservableCollection<PageViewModel>();

        /// <summary>
        /// Application state
        /// </summary>
        private IApplicationState _applicationState;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// <param name="applicationState">Instance of Application state</param>
        public MainViewModel(IApplicationState applicationState)
        {
            _dialogCoordinator = DialogCoordinator.Instance;
            _applicationState = applicationState;
            RegisterMessages();
            RegisterCommands();
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
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
        /// Pages shown into the interface
        /// </summary>
        public ObservableCollection<PageViewModel> Pages
        {
            get { return _pages; }
            set { Set(() => Pages, ref _pages, value); }
        }

        /// <summary>
        /// Specify if settings flyout is open
        /// </summary>
        public bool IsSettingsFlyoutOpen
        {
            get { return _isSettingsFlyoutOpen; }
            set { Set(() => IsSettingsFlyoutOpen, ref _isSettingsFlyoutOpen, value); }
        }

        /// <summary>
        /// Specify if movie flyout is open
        /// </summary>
        public bool IsMovieFlyoutOpen
        {
            get { return _isMovieFlyoutOpen; }
            set { Set(() => IsMovieFlyoutOpen, ref _isMovieFlyoutOpen, value); }
        }

        /// <summary>
        /// Command used to close movie page
        /// </summary>
        public RelayCommand CloseMoviePageCommand { get; private set; }

        /// <summary>
        /// Command used to close the application
        /// </summary>
        public RelayCommand MainWindowClosingCommand { get; private set; }

        /// <summary>
        /// Command used to open application settings
        /// </summary>
        public RelayCommand OpenSettingsCommand { get; private set; }

        /// <summary>
        /// Command used to load tabs
        /// </summary>
        public RelayCommand InitializeAsyncCommand { get; private set; }

        /// <summary>
        /// Load pages asynchronously
        /// </summary>
        private void Load()
        {
            Pages = new ObservableCollection<PageViewModel>
            {
                new MoviePageViewModel(SimpleIoc.Default.GetInstance<IGenresViewModel>(),
                    SimpleIoc.Default.GetInstance<IMovieService>(),
                    SimpleIoc.Default.GetInstance<IMovieHistoryService>(),
                    SimpleIoc.Default.GetInstance<IApplicationState>())
                {
                    Caption = "Movies"
                },
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

        /// <summary>
        /// Register messages
        /// </summary>
        private void RegisterMessages()
        {
            Messenger.Default.Register<ManageExceptionMessage>(this, e => ManageException(e.UnHandledException));

            Messenger.Default.Register<LoadMovieMessage>(this, e => IsMovieFlyoutOpen = true);

            Messenger.Default.Register<PlayMovieMessage>(this, message => DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                IsMovieFlyoutOpen = false;
            }));

            Messenger.Default.Register<StopPlayingMovieMessage>(
                this,
                message =>
                {
                    IsMovieFlyoutOpen = true;
                });

            Messenger.Default.Register<UnhandledExceptionMessage>(this, message => ManageException(message.Exception));
        }

        /// <summary>
        /// Register commands
        /// </summary>
        private void RegisterCommands()
        {
            CloseMoviePageCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new StopPlayingTrailerMessage());
                IsMovieFlyoutOpen = false;
            });

            MainWindowClosingCommand = new RelayCommand(() =>
            {
                if (!Directory.Exists(Constants.MovieDownloads)) return;
                foreach (
                    var filePath in Directory.GetDirectories(Constants.MovieDownloads)
                )
                {
                    try
                    {
                        Logger.Debug(
                            $"Deleting directory: {filePath}");
                        Directory.Delete(filePath, true);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Error while deleting directory: {ex.Message}.");
                    }
                }

                if (!Directory.Exists(Constants.TorrentDownloads)) return;
                foreach (
                    var filePath in Directory.GetFiles(Constants.TorrentDownloads, "*.*", SearchOption.AllDirectories)
                )
                {
                    try
                    {
                        Logger.Debug(
                            $"Deleting file: {filePath}");
                        File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Error while deleting file: {ex.Message}.");
                    }
                }
            });

            OpenSettingsCommand = new RelayCommand(() => IsSettingsFlyoutOpen = true);

            InitializeAsyncCommand = new RelayCommand(async () =>
            {
                Load();
#if !DEBUG
                await StartUpdateProcessAsync();
#endif
            });
        }

        /// <summary>
        /// Look for update then download and apply if any
        /// </summary>
        private async Task StartUpdateProcessAsync()
        {
            var watchStart = Stopwatch.StartNew();

            Logger.Info(
                "Looking for updates...");
            try
            {
                using (var updateManager = await UpdateManager.GitHubUpdateManager(Constants.GithubRepository))
                {
                    var updateInfo = await updateManager.CheckForUpdate();
                    if (updateInfo == null)
                    {
                        Logger.Error(
                            "Problem while trying to check new updates.");
                        return;
                    }

                    if (updateInfo.ReleasesToApply.Any())
                    {
                        Logger.Info(
                            $"A new update has been found!\n Currently installed version: {updateInfo.CurrentlyInstalledVersion?.Version?.Version.Major}.{updateInfo.CurrentlyInstalledVersion?.Version?.Version.Minor}.{updateInfo.CurrentlyInstalledVersion?.Version?.Version.Build} - New update: {updateInfo.FutureReleaseEntry?.Version?.Version.Major}.{updateInfo.FutureReleaseEntry?.Version?.Version.Minor}.{updateInfo.FutureReleaseEntry?.Version?.Version.Build}");

                        await updateManager.DownloadReleases(updateInfo.ReleasesToApply, x => Logger.Info(
                            $"Downloading new update... {x}%"));

                        var latestExe = await updateManager.ApplyReleases(updateInfo, x => Logger.Info(
                            $"Applying... {x}%"));

                        Logger.Info(
                            "A new update has been applied.");

                        var releaseInfos = string.Empty;
                        foreach (var releaseInfo in updateInfo.FetchReleaseNotes())
                        {
                            var info = releaseInfo.Value;

                            var pFrom = info.IndexOf("<p>", StringComparison.InvariantCulture) + "<p>".Length;
                            var pTo = info.LastIndexOf("</p>", StringComparison.InvariantCulture);

                            releaseInfos = string.Concat(releaseInfos, info.Substring(pFrom, pTo - pFrom),
                                Environment.NewLine);
                        }

                        var updateDialog =
                            new UpdateDialog(
                                new UpdateDialogSettings(
                                    LocalizationProviderHelper.GetLocalizedValue<string>("NewUpdateLabel"),
                                    LocalizationProviderHelper.GetLocalizedValue<string>("NewUpdateDescriptionLabel"),
                                    releaseInfos));
                        await _dialogCoordinator.ShowMetroDialogAsync(this, updateDialog);
                        var updateDialogResult = await updateDialog.WaitForButtonPressAsync();
                        await _dialogCoordinator.HideMetroDialogAsync(this, updateDialog);

                        if (!updateDialogResult.Restart) return;

                        Logger.Info(
                            "Restarting...");
                        UpdateManager.RestartApp(Path.Combine(latestExe, "Popcorn.exe"));
                    }
                    else
                    {
                        Logger.Info(
                            "No update available.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(
                    $"Something went wrong when trying to update app. {ex.Message}");
            }

            watchStart.Stop();
            var elapsedStartMs = watchStart.ElapsedMilliseconds;
            Logger.Info(
                "Finished looking for updates.", elapsedStartMs);
        }

        /// <summary>
        /// Display a dialog on unhandled exception
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                Logger.Fatal(ex);
                ManageException(new Exception(LocalizationProviderHelper.GetLocalizedValue<string>("FatalError")));
            }
        }

        /// <summary>
        /// Manage an exception
        /// </summary>
        /// <param name="exception">The exception to manage</param>
        private void ManageException(Exception exception)
        {
            if (_isManagingException)
                return;

            _isManagingException = true;
            IsMovieFlyoutOpen = false;
            IsSettingsFlyoutOpen = false;

            if (exception is WebException || exception is SocketException)
                _applicationState.IsConnectionInError = true;

            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                var exceptionDialog =
                    new ExceptionDialog(
                        new ExceptionDialogSettings(
                            LocalizationProviderHelper.GetLocalizedValue<string>("EmbarrassingError"), exception.Message));
                await _dialogCoordinator.ShowMetroDialogAsync(this, exceptionDialog);
                await exceptionDialog.WaitForButtonPressAsync();
                _isManagingException = false;
                await _dialogCoordinator.HideMetroDialogAsync(this, exceptionDialog);
            });
        }
    }
}