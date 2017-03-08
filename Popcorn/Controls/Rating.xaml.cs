using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Popcorn.Helpers;
using Brush = System.Drawing.Brush;

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

            switch (button.Name)
            {
                case null:
                    return;
                case "StarOne":
                    StarOne.IsChecked = !StarOne.IsChecked;
                    RatingValue = 2;
                    break;
                case "StarTwo":
                    StarOne.IsChecked = !StarOne.IsChecked;
                    StarTwo.IsChecked = !StarTwo.IsChecked;
                    RatingValue = 4;
                    break;
                case "StarThree":
                    button.IsChecked = true;
                    RatingValue = 6;
                    break;
                case "StarFour":
                    button.IsChecked = true;
                    RatingValue = 8;
                    break;
                case "StarFive":
                    button.IsChecked = true;
                    RatingValue = 10;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnMouseEnterStarOne(object sender, MouseEventArgs e)
        {
            if (!StarOne.IsChecked.HasValue || !StarOne.IsChecked.Value)
            {
                var star = StarOne.FindChild<Path>("star");
                star.Fill = Brushes.DarkOrange;
                star.Opacity = 0.8;
            }
        }

        private void OnMouseLeaveStarOne(object sender, MouseEventArgs e)
        {
            if (!StarOne.IsChecked.HasValue || !StarOne.IsChecked.Value)
            {
                var star = StarOne.FindChild<Path>("star");
                star.Fill = Brushes.White;
                star.Opacity = 1.0;
            }
        }

        private void OnMouseEnterStarTwo(object sender, MouseEventArgs e)
        {
            OnMouseEnterStarOne(null, null);

            if (!StarTwo.IsChecked.HasValue || !StarTwo.IsChecked.Value)
            {
                var star = StarTwo.FindChild<Path>("star");
                star.Fill = Brushes.DarkOrange;
                star.Opacity = 0.8;
            }
        }

        private void OnMouseLeaveStarTwo(object sender, MouseEventArgs e)
        {
            OnMouseLeaveStarOne(null, null);

            if (!StarTwo.IsChecked.HasValue || !StarTwo.IsChecked.Value)
            {
                var star = StarTwo.FindChild<Path>("star");
                star.Fill = Brushes.White;
                star.Opacity = 1.0;
            }
        }

        private void OnMouseEnterStarThree(object sender, MouseEventArgs e)
        {
            OnMouseEnterStarTwo(null, null);

            if (!StarThree.IsChecked.HasValue || !StarThree.IsChecked.Value)
            {
                var star = StarThree.FindChild<Path>("star");
                star.Fill = Brushes.DarkOrange;
                star.Opacity = 0.8;
            }
        }

        private void OnMouseLeaveStarThree(object sender, MouseEventArgs e)
        {
            OnMouseLeaveStarTwo(null, null);

            if (!StarThree.IsChecked.HasValue || !StarThree.IsChecked.Value)
            {
                var star = StarThree.FindChild<Path>("star");
                star.Fill = Brushes.White;
                star.Opacity = 1.0;
            }
        }

        private void OnMouseEnterStarFour(object sender, MouseEventArgs e)
        {
            OnMouseEnterStarThree(null, null);

            if (!StarFour.IsChecked.HasValue || !StarFour.IsChecked.Value)
            {
                var star = StarFour.FindChild<Path>("star");
                star.Fill = Brushes.DarkOrange;
                star.Opacity = 0.8;
            }
        }

        private void OnMouseLeaveStarFour(object sender, MouseEventArgs e)
        {
            OnMouseLeaveStarThree(null, null);

            if (!StarFour.IsChecked.HasValue || !StarFour.IsChecked.Value)
            {
                var star = StarFour.FindChild<Path>("star");
                star.Fill = Brushes.White;
                star.Opacity = 1.0;
            }
        }

        private void OnMouseEnterStarFive(object sender, MouseEventArgs e)
        {
            OnMouseEnterStarFour(null, null);

            if (!StarFive.IsChecked.HasValue || !StarFive.IsChecked.Value)
            {
                var star = StarFive.FindChild<Path>("star");
                star.Fill = Brushes.DarkOrange;
                star.Opacity = 0.8;
            }
        }

        private void OnMouseLeaveStarFive(object sender, MouseEventArgs e)
        {
            OnMouseLeaveStarFour(null, null);

            if (!StarFive.IsChecked.HasValue || !StarFive.IsChecked.Value)
            {
                var star = StarFive.FindChild<Path>("star");
                star.Fill = Brushes.White;
                star.Opacity = 1.0;
            }
        }
    }
}