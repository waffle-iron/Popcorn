using System.Diagnostics;
using System.IO;
using GalaSoft.MvvmLight.Threading;
using NLog;
using WPFLocalizeExtension.Engine;

namespace Popcorn
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the App class.
        /// </summary>
        static App()
        {
            Logger.Info(
                "Popcorn starting...");
            var watchStart = Stopwatch.StartNew();

            Directory.CreateDirectory(Helpers.Constants.Logging);

            DispatcherHelper.Initialize();

            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;

            watchStart.Stop();
            var elapsedStartMs = watchStart.ElapsedMilliseconds;
            Logger.Info(
                $"Popcorn started in {elapsedStartMs} milliseconds.");
        }
    }
}