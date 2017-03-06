using GalaSoft.MvvmLight;

namespace Popcorn.ViewModels.Pages
{
    /// <summary>
    /// Represents a page
    /// </summary>
    public class PageViewModel : ViewModelBase
    {
        /// <summary>
        /// <see cref="Caption"/>
        /// </summary>
        private string _caption;

        /// <summary>
        /// Tab caption 
        /// </summary>
        public string Caption
        {
            get { return _caption; }
            set { Set(ref _caption, value); }
        }
    }
}