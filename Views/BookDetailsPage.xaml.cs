using System;

using BookLibrary1.ViewModels;

using Windows.UI.Xaml.Controls;

namespace BookLibrary1.Views
{
    public sealed partial class BookDetailsPage : Page
    {
        public BookDetailsViewModel ViewModel { get; } = new BookDetailsViewModel();

        public BookDetailsPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, KeyboardAccelerators);
        }

    }
}
