using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Popcorn.ViewModels.Pages.Home.Anime
{
    public class AnimePageViewModel : ObservableObject, IPageViewModel
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
