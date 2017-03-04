using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Popcorn.Controls
{
    /// <summary>
    /// Interaction logic for Rating.xaml
    /// </summary>
    public partial class Rating
    {
        /// <summary>
        /// Max rating value
        /// </summary>
        private const int Max = 10;

        /// <summary>
        /// Rating property
        /// </summary>
        public static readonly DependencyProperty RatingValueProperty = DependencyProperty.Register("RatingValue",
            typeof (double), typeof (Rating),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RatingChanged));

        /// <summary>
        /// Star buttons property
        /// </summary>
        public static readonly DependencyProperty StarButtonsEnabledProperty =
            DependencyProperty.Register("StarButtonsEnabled",
                typeof (bool), typeof (Rating),
                new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Initialize a new instance of Rating
        /// </summary>
        public Rating()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Are stars clickable
        /// </summary>
        public bool StarButtonsEnabled
        {
            get { return (bool) GetValue(StarButtonsEnabledProperty); }
            set { SetValue(StarButtonsEnabledProperty, value); }
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
            var rating = sender as Rating;
            if (rating == null)
                return;

            var newval = Convert.ToInt32((double) e.NewValue);
            newval /= 2;
            var childs = ((Grid) (rating.Content)).Children;

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

        /// <summary>
        /// Toggle star on click event
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">Event args</param>
        private void ToggleStar(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            if (button == null)
                return;

            var tag = button.Tag as string;
            switch (tag)
            {
                case null:
                    return;
                case "1":
                    RatingValue = 2;
                    button.IsChecked = true;
                    break;
                case "2":
                    RatingValue = 4;
                    button.IsChecked = true;
                    break;
                case "3":
                    RatingValue = 6;
                    button.IsChecked = true;
                    break;
                case "4":
                    RatingValue = 8;
                    button.IsChecked = true;
                    break;
                case "5":
                    RatingValue = 10;
                    button.IsChecked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}