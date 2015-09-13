using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Diagnostics;
using NLog;
using Popcorn.Services.Language;

namespace Popcorn.Models.Localization
{
    /// <summary>
    /// Language
    /// </summary>
    public class Language : ObservableObject
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Services used to interacts with languages
        /// </summary>
        private readonly ILanguageService _languageService;

        private ICollection<ILanguage> _languages;

        /// <summary>
        /// Available languages of the application
        /// </summary>
        public ICollection<ILanguage> Languages
        {
            get { return _languages; }
            set { Set(() => Languages, ref _languages, value); }
        }

        private ILanguage _currentLanguages;

        /// <summary>
        /// Current language used in the application
        /// </summary>
        public ILanguage CurrentLanguage
        {
            get { return _currentLanguages; }
            set
            {
                Task.Run(async () =>
                {
                    await _languageService.SetCurrentLanguageAsync(value);
                });
                Set(() => CurrentLanguage, ref _currentLanguages, value);
            }
        }

        /// <summary>
        /// Initialize a new instance of Language
        /// </summary>
        public Language(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        /// <summary>
        /// Load languages
        /// </summary>
        public async Task LoadLanguages()
        {
            var watchStart = Stopwatch.StartNew();

            CurrentLanguage = await _languageService.GetCurrentLanguageAsync();
            Languages = await _languageService.GetAvailableLanguagesAsync();

            watchStart.Stop();
            var elapsedLanguageMs = watchStart.ElapsedMilliseconds;
            Logger.Info(
                "Languages loaded in {0} milliseconds.", elapsedLanguageMs);
        }
    }
}