using System.Collections.Generic;
using System.Threading.Tasks;
using Popcorn.Models.Localization;

namespace Popcorn.Services.Language
{
    public interface ILanguageService
    {
        /// <summary>
        ///     Get all available languages from the database
        /// </summary>
        /// <returns>All available languages</returns>
        Task<ICollection<ILanguage>> GetAvailableLanguagesAsync();

        /// <summary>
        ///     Get the current language of the application
        /// </summary>
        /// <returns>Current language</returns>
        Task<ILanguage> GetCurrentLanguageAsync();

        /// <summary>
        ///     Get the current language of the application
        /// </summary>
        /// <param name="language">Language to set</param>
        Task SetCurrentLanguageAsync(ILanguage language);
    }
}