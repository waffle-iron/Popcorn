using GalaSoft.MvvmLight;

namespace Popcorn.Models.Localization
{
    /// <summary>
    /// French language
    /// </summary>
    public sealed class FrenchLanguage : ObservableObject, ILanguage
    {
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
        /// Initialize a new instance of FrenchLanguage
        /// </summary>
        public FrenchLanguage()
        {
            LocalizedName = "Français";
            EnglishName = "French";
            Culture = "fr";
        }

        /// <summary>
        /// Check equality based on is localized name
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var item = obj as FrenchLanguage;

            return item != null && LocalizedName.Equals(item.LocalizedName);
        }

        /// <summary>
        /// Get hash code based on it localized name
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return LocalizedName.GetHashCode();
        }
    }
}