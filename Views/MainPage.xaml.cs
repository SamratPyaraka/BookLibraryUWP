using System;
using System.Collections.Generic;
using System.Linq;
using BookLibrary1.Helpers;
using BookLibrary1.Model;
using BookLibrary1.Services;
using BookLibrary1.ViewModels;
using Microsoft.Toolkit.Uwp;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using static BookLibrary1.ViewModels.MainViewModel;

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
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        
        

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int selectedIndex = gridView.SelectedIndex;
                //
                var books = (GridView)sender;
                var x = (Books)books.SelectedItem;
                AppSettings.BookID = x.BookID;
                NavigationService.Navigate(typeof(BookDetailsPage));
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "MainViewModel->GridView_SelectionChanged");
            }
        }

        private void gridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int selectedIndex = gridView.SelectedIndex;
                //
                var books = (GridView)sender;
                var x = (Books)books.SelectedItem;
                AppSettings.BookID = x.BookID;
                NavigationService.Navigate(typeof(BookDetailsPage));
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "MainViewModel->GridView_SelectionChanged");
            }
        }
        public IncrementalLoadingCollection<BookSource, Books> ListOfBooks { get; set; }
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            try
            {
                if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    if (textSearched.Text != null)
                    {
                        if (ListOfBooks == null)
                            ListOfBooks = ViewModel.GetBooksList();

                        var books = PopulateSearchedBooks(textSearched.Text);
                        if (books != null && books.Count > 0)
                        {
                            sender.ItemsSource = books;
                        }
                        else
                            sender.ItemsSource = new string[] { "No results found" };
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "MainPage->AutoSuggestBox_TextChanged");
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            try
            {
                if (!string.IsNullOrEmpty(args.QueryText))
                {
                    //Do a fuzzy search based on the text
                    var suggestions = PopulateSearchedBooks(sender.Text);
                    if (suggestions.Count > 0)
                    {
                        sender.ItemsSource = suggestions.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "MainPage->AutoSuggestBox_QuerySubmitted");
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            try
            {
                if (args.SelectedItem is Books control)
                {
                    sender.Text = control.Title;
                    AppSettings.BookID = Convert.ToInt32(control.BookID);
                    NavigationService.Navigate(typeof(BookDetailsPage));
                }
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "MainPage->AutoSuggestBox_SuggestionChosen");
            }
        }

        public List<Books> PopulateSearchedBooks(string textSearched)
        {
            List<Books> searchedBooks = new List<Books>();
            try
            {
                if (ListOfBooks != null && ListOfBooks.Count > 0)
                {
                    searchedBooks = ListOfBooks.Where(x => x.Title.ToLower().Contains(textSearched)).ToList();

                    if (searchedBooks.Count > 0)
                    {
                        return searchedBooks;
                    }
                }

            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "MainPage->AutoSuggestBox_SuggestionChosen");
            }

            return searchedBooks;
        }
    }
}
