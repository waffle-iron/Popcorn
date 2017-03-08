using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Popcorn.Controls.Movie
{
    /// <summary>
    /// Interaction logic for MovieGenres.xaml
    /// </summary>
    public partial class MovieGenres
    {
        /// <summary>
        /// Genres property
        /// </summary>
        public static readonly DependencyProperty GenresProperty =
            DependencyProperty.Register("Genres",
                typeof (IEnumerable<string>), typeof (MovieGenres),
                new PropertyMetadata(null, OnGenresChanged));

        /// <summary>
        /// Initialize a new instance of MovieGenres
        /// </summary>
        public MovieGenres()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The movie runtime
        /// </summary>
        public IEnumerable<string> Genres
        {
            private get { return (IEnumerable<string>) GetValue(GenresProperty); }
            set { SetValue(GenresProperty, value); }
        }

        /// <summary>
        /// On genres changed
        /// </summary>
        /// <param name="d">Dependency object</param>
        /// <param name="e">Event args</param>
        private static void OnGenresChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var movieGenres = d as MovieGenres;
            movieGenres?.DisplayMovieGenres();
        }

        /// <summary>
        /// Display movie genres
        /// </summary>
        private void DisplayMovieGenres()
        {
            var index = 0;
            if (Genres == null)
                return;

            DisplayText.Text = string.Empty;
            foreach (var genre in Genres)
            {
                index++;
                DisplayText.Text += genre;
                // Add the comma at the end of each genre.
                if (index != Genres.Count())
                    DisplayText.Text += ", ";
            }
        }
    }
}