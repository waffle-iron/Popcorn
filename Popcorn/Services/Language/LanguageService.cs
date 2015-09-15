using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using NLog;
using Popcorn.Entity;
using Popcorn.Messaging;
using Popcorn.Models.Localization;
using Popcorn.Services.Movie;
using Popcorn.Services.Settings;
using WPFLocalizeExtension.Engine;

namespace Popcorn.Services.Language
{
    /// <summary>
    ///     Services used to interacts with languages
    /// </summary>
    public class LanguageService : ILanguageService
    {
        /// <summary>
        ///     Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Services used to interact with movies
        /// </summary>
        private readonly IMovieService _movieService;

        /// <summary>
        ///     Services used to interacts with application settings
        /// </summary>
        private readonly ISettingsService _settingsService;

        /// <summary>
        ///     Initialize a new instance of LanguageService class
        /// </summary>
        public LanguageService(ISettingsService settingsService, IMovieService movieService)
        {
            _settingsService = settingsService;
            _movieService = movieService;
        }

        /// <summary>
        ///     Get all available languages from the database
        /// </summary>
        /// <returns>All available languages</returns>
        public async Task<ICollection<ILanguage>> GetAvailableLanguagesAsync()
        {
            var watch = Stopwatch.StartNew();

            ICollection<ILanguage> availableLanguages;

            using (var context = new ApplicationDbContext())
            {
                var applicationSettings = await context.Settings.FirstOrDefaultAsync();
                if (applicationSettings == null)
                {
                    await _settingsService.CreateApplicationSettingsAsync();
                    applicationSettings = await context.Settings.FirstOrDefaultAsync();
                }

                var languages = applicationSettings.Languages;
                availableLanguages = new List<ILanguage>();
                foreach (var language in languages)
                {
                    switch (language.Culture)
                    {
                        case "en":
                            availableLanguages.Add(new EnglishLanguage());
                            break;
                        case "fr":
                            availableLanguages.Add(new FrenchLanguage());
                            break;
                        default:
                            availableLanguages.Add(new EnglishLanguage());
                            break;
                    }
                }
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Logger.Debug(
                $"GetAvailableLanguagesAsync in {elapsedMs} milliseconds.");

            return availableLanguages;
        }

        /// <summary>
        ///     Get the current language of the application
        /// </summary>
        /// <returns>Current language</returns>
        public async Task<ILanguage> GetCurrentLanguageAsync()
        {
            ILanguage currentLanguage = null;

            var watch = Stopwatch.StartNew();
            using (var context = new ApplicationDbContext())
            {
                var applicationSettings = await context.Settings.FirstOrDefaultAsync();
                if (applicationSettings == null)
                {
                    await _settingsService.CreateApplicationSettingsAsync();
                    applicationSettings = await context.Settings.FirstOrDefaultAsync();
                }

                var language = applicationSettings.Languages.FirstOrDefault(a => a.IsCurrentLanguage);
                if (language != null)
                {
                    switch (language.Culture)
                    {
                        case "en":
                            currentLanguage = new EnglishLanguage();
                            break;
                        case "fr":
                            currentLanguage = new FrenchLanguage();
                            break;
                        default:
                            currentLanguage = new EnglishLanguage();
                            break;
                    }
                }
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Logger.Debug(
                $"GetCurrentLanguageAsync in {elapsedMs} milliseconds.");

            return currentLanguage;
        }

        /// <summary>
        ///     Get the current language of the application
        /// </summary>
        /// <param name="language">Language to set</param>
        public async Task SetCurrentLanguageAsync(ILanguage language)
        {
            if (language == null) throw new ArgumentNullException(nameof(language));
            var watch = Stopwatch.StartNew();

            using (var context = new ApplicationDbContext())
            {
                var applicationSettings = await context.Settings.FirstOrDefaultAsync();
                if (applicationSettings == null)
                {
                    await _settingsService.CreateApplicationSettingsAsync();
                    applicationSettings = await context.Settings.FirstOrDefaultAsync();
                }

                var currentLanguage = applicationSettings.Languages.First(a => a.Culture == language.Culture);
                currentLanguage.IsCurrentLanguage = true;
                foreach (var lang in applicationSettings.Languages.Where(a => a.Culture != language.Culture))
                {
                    lang.IsCurrentLanguage = false;
                }

                context.Settings.AddOrUpdate(applicationSettings);
                await context.SaveChangesAsync();
                ChangeLanguage(language);
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Logger.Debug(
                $"SetCurrentLanguageAsync ({language.LocalizedName}) in {elapsedMs} milliseconds.");
        }

        /// <summary>
        ///     Change language
        /// </summary>
        /// <param name="language"></param>
        private void ChangeLanguage(ILanguage language)
        {
            if (language == null) throw new ArgumentNullException(nameof(language));
            _movieService.ChangeTmdbLanguage(language);
            LocalizeDictionary.Instance.Culture = new CultureInfo(language.Culture);
            Messenger.Default.Send(new ChangeLanguageMessage());
        }
    }
}