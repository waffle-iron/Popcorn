using System.ComponentModel;
using Meta.Vlc.Wpf;

namespace Popcorn.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On window closing, release VLC instance
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            ApiManager.ReleaseAll();
            base.OnClosing(e);
        }
    }
}