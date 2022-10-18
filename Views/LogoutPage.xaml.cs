using System;

using BookLibrary1.ViewModels;

using Windows.UI.Xaml.Controls;

namespace BookLibrary1.Views
{
    public sealed partial class LogoutPage : Page
    {
        public LogoutViewModel ViewModel { get; } = new LogoutViewModel();

        public LogoutPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, KeyboardAccelerators);
        }
    }
}
