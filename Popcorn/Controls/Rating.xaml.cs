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
        public static readonly DependencyProperty RatingValueProperty = DependencyProperty.Register("RatingValue",
            typeof (int), typeof (Rating),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RatingChanged));

        public static readonly DependencyProperty StartButtonsEnabledProperty =
            DependencyProperty.Register("StarButtonsEnabled",
                typeof (bool), typeof (Rating),
                new FrameworkPropertyMetadata(false));

        private const int Max = 10;

        public bool StarButtonsEnabled
        {
            get { return (bool) GetValue(StartButtonsEnabledProperty); }
            set { SetValue(StartButtonsEnabledProperty, value); }
        }

        public int RatingValue
        {
            set
            {
                if (value < 0)
                {
                    SetValue(RatingValueProperty, 0);
                }
                else if (value > Max)
                {
                    SetValue(RatingValueProperty, Max);
                }
                else
                {
                    SetValue(RatingValueProperty, value);
                }
            }
        }

        private static void RatingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var rating = sender as Rating;
            if (rating == null)
            {
                return;
            }

            var newval = (int) e.NewValue;
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
        /// Constructor
        /// </summary>
        public Rating()
        {
            InitializeComponent();
        }

        private void ToggleStar(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            if (button == null)
            {
                return;
            }

            var tag = button.Tag as string;
            switch (tag)
            {
                case null:
                    return;
                case "1":
                    RatingValue = 2;
                    break;
                case "2":
                    RatingValue = 4;
                    break;
                case "3":
                    RatingValue = 6;
                    break;
                case "4":
                    RatingValue = 8;
                    break;
                case "5":
                    RatingValue = 10;
                    break;
            }
        }
    }
}