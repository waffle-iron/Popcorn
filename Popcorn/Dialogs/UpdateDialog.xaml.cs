using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Popcorn.Helpers;

namespace Popcorn.Dialogs
{
    public class UpdateDialogSettings : MetroDialogSettings
    {
        public UpdateDialogSettings(string title, string message, string releaseNotes)
        {
            Title = title;
            Message = message;
            ReleaseNotes = releaseNotes;
        }

        public string Title { get; }

        public string Message { get; }

        public string ReleaseNotes { get; }
    }

    public class UpdateDialogData
    {
        public bool Restart { get; set; }
    }

    public partial class UpdateDialog
    {
        internal UpdateDialog(UpdateDialogSettings settings)
        {
            InitializeComponent();
            Message = settings.Message;
            Title = settings.Title;
            ReleaseNotes = settings.ReleaseNotes;
        }

        internal Task<UpdateDialogData> WaitForButtonPressAsync()
        {
            TaskCompletionSource<UpdateDialogData> tcs = new TaskCompletionSource<UpdateDialogData>();

            RoutedEventHandler restartHandler = null;
            KeyEventHandler restartKeyHandler = null;

            RoutedEventHandler laterHandler = null;
            KeyEventHandler laterKeyHandler = null;

            KeyEventHandler escapeKeyHandler = null;

            Action cleanUpHandlers = null;

            var cancellationTokenRegistration = DialogSettings.CancellationToken.Register(() =>
            {
                cleanUpHandlers();
                tcs.TrySetResult(null);
            });

            cleanUpHandlers = () => {
                KeyDown -= escapeKeyHandler;

                PART_RestartButton.Click -= restartHandler;

                PART_RestartButton.KeyDown -= restartKeyHandler;

                PART_LaterButton.Click -= laterHandler;

                PART_LaterButton.KeyDown -= laterKeyHandler;

                cancellationTokenRegistration.Dispose();
            };

            escapeKeyHandler = (sender, e) =>
            {
                if (e.Key != Key.Escape) return;
                cleanUpHandlers();

                tcs.TrySetResult(null);
            };

            restartKeyHandler = (sender, e) =>
            {
                if (e.Key != Key.Enter) return;
                cleanUpHandlers();

                tcs.TrySetResult(new UpdateDialogData
                {
                    Restart = true
                });
            };

            restartHandler = (sender, e) =>
            {
                cleanUpHandlers();

                tcs.TrySetResult(new UpdateDialogData
                {
                    Restart = true
                });

                e.Handled = true;
            };

            laterKeyHandler = (sender, e) =>
            {
                if (e.Key != Key.Enter) return;
                cleanUpHandlers();

                tcs.TrySetResult(new UpdateDialogData
                {
                    Restart = false
                });
            };

            laterHandler = (sender, e) =>
            {
                cleanUpHandlers();

                tcs.TrySetResult(new UpdateDialogData
                {
                    Restart = false
                });

                e.Handled = true;
            };

            PART_RestartButton.KeyDown += restartKeyHandler;

            PART_LaterButton.KeyDown += laterKeyHandler;

            KeyDown += escapeKeyHandler;

            PART_RestartButton.Click += restartHandler;

            PART_LaterButton.Click += laterHandler;

            return tcs.Task;
        }

        protected override void OnLoaded()
        {
            switch (DialogSettings.ColorScheme)
            {
                case MetroDialogColorScheme.Accented:
                    PART_RestartButton.Style = FindResource("AccentedDialogHighlightedSquareButton") as Style;
                    PART_LaterButton.Style = FindResource("AccentedDialogHighlightedSquareButton") as Style;
                    break;
            }
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(UpdateDialog), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ReleaseNotesProperty = DependencyProperty.Register("ReleaseNotes", typeof(string), typeof(UpdateDialog), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty RestartButtonTextProperty = DependencyProperty.Register("RestartButtonText", typeof(string), typeof(UpdateDialog), new PropertyMetadata(LocalizationProviderHelper.GetLocalizedValue<string>("NowLabel")));
        public static readonly DependencyProperty LaterButtonTextProperty = DependencyProperty.Register("LaterButtonText", typeof(string), typeof(UpdateDialog), new PropertyMetadata(LocalizationProviderHelper.GetLocalizedValue<string>("LaterLabel")));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public string ReleaseNotes
        {
            get { return (string)GetValue(ReleaseNotesProperty); }
            set { SetValue(ReleaseNotesProperty, value); }
        }

        public string RestartButtonText
        {
            get { return (string)GetValue(RestartButtonTextProperty); }
            set { SetValue(RestartButtonTextProperty, value); }
        }

        public string LaterButtonText
        {
            get { return (string)GetValue(LaterButtonTextProperty); }
            set { SetValue(LaterButtonTextProperty, value); }
        }
    }
}
