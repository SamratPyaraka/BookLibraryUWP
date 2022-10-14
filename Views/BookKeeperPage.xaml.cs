using System;
using BookLibrary1.Helpers;
using BookLibrary1.Model;
using BookLibrary1.Services;
using BookLibrary1.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Controls;

namespace BookLibrary1.Views
{
    public sealed partial class BookKeeperPage : Page
    {
        public BookKeeperViewModel ViewModel { get; } = new BookKeeperViewModel();

        public BookKeeperPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, KeyboardAccelerators);

        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //
                var books = (DataGrid)sender;
                var x = (BookRecords)books.SelectedItem;
                AppSettings.BookID = x.BookID;
                NavigationService.Navigate(typeof(BookDetailsPage));
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "BookKeeperViewModel->GridView_SelectionChanged");
            }
        }
    }
}
