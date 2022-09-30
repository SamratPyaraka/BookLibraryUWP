using System;

using BookLibrary1.ViewModels;

using Windows.UI.Xaml.Controls;

namespace BookLibrary1.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
