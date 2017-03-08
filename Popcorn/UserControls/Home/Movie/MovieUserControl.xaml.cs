using System.Windows;
using System.Windows.Input;
using Popcorn.Helpers;

namespace Popcorn.UserControls.Home.Movie
{
    /// <summary>
    /// Logique d'interaction pour MovieUserControl.xaml
    /// </summary>
    public partial class MovieUserControl
    {
        public MovieUserControl()
        {
            InitializeComponent();
            Loaded += MovieUserControl_Loaded;
        }

        private void MovieUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = System.Windows.Window.GetWindow(this);
            if (window == null) return;

            window.PreviewMouseDown += OnPreviewMouseDown;
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            SplitView.IsPaneOpen = element?.FindParentElement("Pane") != null;
        }
    }
}