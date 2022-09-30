using System;

using BookLibrary1.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BookLibrary1.Views
{
    public sealed partial class ListDetailsPage : Page
    {
        public ListDetailsViewModel ViewModel { get; } = new ListDetailsViewModel();

        public ListDetailsPage()
        {
            InitializeComponent();
            Loaded += ListDetailsPage_Loaded;
        }

        private async void ListDetailsPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(ListDetailsViewControl.ViewState);
        }
    }
}
