using System;
using System.Windows;
using Popcorn.Helpers;

namespace Popcorn.Controls
{
    /// <summary>
    ///     Interaction logic for DownloadMovieProgress.xaml
    /// </summary>
    public partial class DownloadMovieProgress
    {
        /// <summary>
        ///     Download progress property
        /// </summary>
        public static readonly DependencyProperty DownloadProgressProperty =
            DependencyProperty.Register("DownloadProgress",
                typeof (double), typeof (DownloadMovieProgress),
                new PropertyMetadata(0d, OnDownloadProgressChanged));

        /// <summary>
        ///     Download rate property
        /// </summary>
        public static readonly DependencyProperty DownloadRateProperty =
            DependencyProperty.Register("DownloadRate",
                typeof (double), typeof (DownloadMovieProgress),
                new PropertyMetadata(0d));

        /// <summary>
        ///     Movie title property
        /// </summary>
        public static readonly DependencyProperty MovieTitleProperty =
            DependencyProperty.Register("MovieTitle",
                typeof (string), typeof (DownloadMovieProgress),
                new PropertyMetadata(string.Empty));

        /// <summary>
        ///     Initialize a new instance of DownloadMovieProgress
        /// </summary>
        public DownloadMovieProgress()
        {
            InitializeComponent();
            DisplayText.Text =
                $"{LocalizationProviderHelper.GetLocalizedValue<string>("BufferingLabel")} : {Math.Round(DownloadProgress*50d, 0)} % ({DownloadRate} kB/s)";
        }

        /// <summary>
        ///     The movie download progress
        /// </summary>
        public double DownloadProgress
        {
            private get { return (double) GetValue(DownloadProgressProperty); }
            set { SetValue(DownloadProgressProperty, value); }
        }

        /// <summary>
        ///     The movie download rate
        /// </summary>
        public double DownloadRate
        {
            private get { return (double) GetValue(DownloadRateProperty); }
            set { SetValue(DownloadRateProperty, value); }
        }

        /// <summary>
        ///     The movie title
        /// </summary>
        public string MovieTitle
        {
            private get { return (string) GetValue(MovieTitleProperty); }
            set { SetValue(MovieTitleProperty, value); }
        }

        /// <summary>
        ///     On download progress changed
        /// </summary>
        /// <param name="d">Dependency object</param>
        /// <param name="e">Event args</param>
        private static void OnDownloadProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var downloadMovieProgress = d as DownloadMovieProgress;
            downloadMovieProgress?.DisplayDownloadProgress();
        }

        /// <summary>
        ///     Display download progress
        /// </summary>
        private void DisplayDownloadProgress()
        {
            if (DownloadProgress >= 2.0)
                DisplayText.Text =
                    $"{LocalizationProviderHelper.GetLocalizedValue<string>("CurrentlyPlayingLabel")} : {MovieTitle}";
            else
                DisplayText.Text = DownloadRate >= 1000.0
                    ? $"{LocalizationProviderHelper.GetLocalizedValue<string>("BufferingLabel")} : {Math.Round(DownloadProgress*50d, 0)} % ({DownloadRate/1000d} MB/s)"
                    : $"{LocalizationProviderHelper.GetLocalizedValue<string>("BufferingLabel")} : {Math.Round(DownloadProgress*50d, 0)} % ({DownloadRate} kB/s)";
        }
    }
}