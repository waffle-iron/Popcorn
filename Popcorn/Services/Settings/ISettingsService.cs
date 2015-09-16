using System.Threading.Tasks;

namespace Popcorn.Services.Settings
{
    public interface ISettingsService
    {
        /// <summary>
        /// Scaffold application settings if empty
        /// </summary>
        Task CreateApplicationSettingsAsync();
    }
}