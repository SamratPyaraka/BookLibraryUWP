using System;
using BookLibrary1.Model;
using BookLibrary1.Services;
using BookLibrary1.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BookLibrary1.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, KeyboardAccelerators);
        }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = gridView.SelectedIndex;
            //
            var books = (GridView)sender;
            var x = (Books)books.SelectedItem;
            AppSettings.BookID = x.BookID;
            NavigationService.Navigate(typeof(BookDetailsPage));
        }
    }
}
