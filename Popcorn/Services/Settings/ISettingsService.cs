using System.Threading.Tasks;

namespace Popcorn.Services.Settings
{
    public interface ISettingsService
    {
        /// <summary>
        /// Scaffold Settings table on database if empty
        /// </summary>
        Task CreateApplicationSettingsAsync();
    }
}
