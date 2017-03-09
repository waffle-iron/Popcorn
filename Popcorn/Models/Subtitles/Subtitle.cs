using GalaSoft.MvvmLight;

namespace Popcorn.Models.Subtitles
{
    public class Subtitle : ObservableObject
    {
        private string _filePath;
        private OSDBnet.Subtitle _subtitle;

        /// <summary>
        /// Subtitle
        /// </summary>
        public OSDBnet.Subtitle Sub
        {
            get { return _subtitle; }
            set { Set(() => Sub, ref _subtitle, value); }
        }

        /// <summary>
        /// Subtitle file path
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set { Set(() => FilePath, ref _filePath, value); }
        }
    }
}