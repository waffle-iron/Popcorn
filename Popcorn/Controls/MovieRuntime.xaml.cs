using System;
using System.Globalization;
using System.Windows;

namespace Popcorn.Controls
{
    /// <summary>
    /// Interaction logic for MovieRuntime.xaml
    /// </summary>
    public partial class MovieRuntime
    {
        /// <summary>
        /// Runtime property
        /// </summary>
        public static readonly DependencyProperty RuntimeProperty =
            DependencyProperty.Register("Runtime",
                typeof (double), typeof (MovieRuntime),
                new PropertyMetadata(0.0, OnRuntimeChanged));

        /// <summary>
        /// The movie runtime
        /// </summary>
        public double Runtime
        {
            private get { return (double) GetValue(RuntimeProperty); }
            set { SetValue(RuntimeProperty, value); }
        }

        /// <summary>
        /// Initialize a new instance of MovieRuntime
        /// </summary>
        public MovieRuntime()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On movie runtime changed
        /// </summary>
        /// <param name="d">Dependency object</param>
        /// <param name="e">Event args</param>
        private static void OnRuntimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var movieRuntime = d as MovieRuntime;
            movieRuntime?.DisplayMovieRuntime();
        }

        /// <summary>
        /// Display movie runtime
        /// </summary>
        private void DisplayMovieRuntime()
        {
            var result = Convert.ToDouble(Runtime, CultureInfo.InvariantCulture);
            if (!(result >= 60.0)) return;
            var hours = result/60.0;
            var minutes = result%60.0;

            DisplayText.Text = minutes < 10.0 ? $"{Math.Floor(hours)}h0{minutes}" : $"{Math.Floor(hours)}h{minutes}";
        }
    }
}