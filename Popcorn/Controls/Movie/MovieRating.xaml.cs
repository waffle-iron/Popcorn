using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Popcorn.Controls.Movie
{
    /// <summary>
    /// Interaction logic for Rating.xaml
    /// </summary>
    public partial class MovieRating
    {
        /// <summary>
        /// Max rating value
        /// </summary>
        private const int Max = 10;

        /// <summary>
        /// Rating property
        /// </summary>
        public static readonly DependencyProperty RatingValueProperty = DependencyProperty.Register("RatingValue",
            typeof(double), typeof(MovieRating),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RatingChanged));

        /// <summary>
        /// Initialize a new instance of Rating
        /// </summary>
        public MovieRating()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Rating property
        /// </summary>
        public double RatingValue
        {
            set
            {
                if (value < 0)
                    SetValue(RatingValueProperty, 0);
                else if (value > Max)
                    SetValue(RatingValueProperty, Max);
                else
                    SetValue(RatingValueProperty, value);
            }
        }

        /// <summary>
        /// Set IsChecked for each star on rating changed
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event args</param>
        private static void RatingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var rating = sender as MovieRating;
            if (rating == null)
                return;

            var newval = Convert.ToInt32((double)e.NewValue);
            newval /= 2;
            var childs = ((Grid)(rating.Content)).Children;

            ToggleButton button;

            for (var i = 0; i < newval; i++)
            {
                button = childs[i] as ToggleButton;
                if (button != null)
                    button.IsChecked = true;
            }

            for (var i = newval; i < childs.Count; i++)
            {
                button = childs[i] as ToggleButton;
                if (button != null)
                    button.IsChecked = false;
            }
        }
    }
}