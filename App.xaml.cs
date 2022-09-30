using System;
using System.Diagnostics;
using BookLibrary1.Services;
using BookLibrary1.Views;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BookLibrary1
{
    public sealed partial class App : Application
    {
        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {
            InitializeComponent();
            UnhandledException += OnAppUnhandledException;

            // Deferred execution until used. Check https://docs.microsoft.com/dotnet/api/system.lazy-1 for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol)
            {
                // Extracts the authorization response URI from the arguments.
                ProtocolActivatedEventArgs protocolArgs = (ProtocolActivatedEventArgs)args;
                Uri uri = protocolArgs.Uri;
                Debug.WriteLine("Authorization Response: " + uri.AbsoluteUri);

                // Gets the current frame, making one if needed.
                var frame = Window.Current.Content as Frame;
                if (frame == null)
                    frame = new Frame();

                // Opens the URI for "navigation" (handling) on the MainPage.
                frame.Navigate(typeof(LoginDetailsPage), uri);
                Window.Current.Content = frame;
                Window.Current.Activate();
            }


            await ActivationService.ActivateAsync(args);

        }


        private void OnAppUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // TODO: Please log and handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/uwp/api/windows.ui.xaml.application.unhandledexception
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(Views.MainPage), new Lazy<UIElement>(CreateShell));
        }

       

        private UIElement CreateShell()
        {
            return new Views.ShellPage();
        }
    }
}
