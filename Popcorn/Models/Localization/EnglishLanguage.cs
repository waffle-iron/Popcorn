using GalaSoft.MvvmLight;

namespace Popcorn.Models.Localization
{
    /// <summary>
    /// English language
    /// </summary>
    public sealed class EnglishLanguage : ObservableObject, ILanguage
    {
        /// <summary>
        /// Initialize a new instance of EnglishLanguage
        /// </summary>
        public EnglishLanguage()
        {
            LocalizedName = "English";
            EnglishName = "English";
            Culture = "en";
        }

        /// <summary>
        /// Language's name
        /// </summary>
        public string LocalizedName { get; }

        /// <summary>
        /// English language's name
        /// </summary>
        public string EnglishName { get; }

        /// <summary>
        /// Language's culture
        /// </summary>
        public string Culture { get; }

        /// <summary>
        /// Check equality based on is localized name
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var item = obj as EnglishLanguage;

            return item != null && LocalizedName.Equals(item.LocalizedName);
        }

        /// <summary>
        /// Get hash code based on it localized name
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() => LocalizedName.GetHashCode();
    }
}