using System;

using BookLibrary1.ViewModels;

using Windows.UI.Xaml.Controls;

namespace BookLibrary1.Views
{
    public sealed partial class CheckoutPage : Page
    {
        public CheckoutViewModel ViewModel { get; } = new CheckoutViewModel();

        public CheckoutPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, KeyboardAccelerators);
        }
    }
}
