﻿using System;
using System.Collections.Generic;
using System.Linq;
using BookLibrary1.Helpers;
using BookLibrary1.Model;
using BookLibrary1.Services;
using BookLibrary1.ViewModels;
using Microsoft.Toolkit.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using static System.Reflection.Metadata.BlobBuilder;
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
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (textSearched.Text != null)
                {
                    if (ListOfBooks == null)
                        ListOfBooks = ViewModel.GetBooksList();

                    var books = PopulateSearchedBooks(textSearched.Text);
                    if (books.Count > 0)
                    {
                        sender.ItemsSource = books;
                    }
                    else
                        sender.ItemsSource = new string[] { "No results found" };
                }

            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
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

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is Books control)
            {
                sender.Text = control.BookName;
                AppSettings.BookID = Convert.ToInt32(control.BookID);
                NavigationService.Navigate(typeof(BookDetailsPage));
            }
        }

        public List<Books> PopulateSearchedBooks(string textSearched)
        {
            List<Books> searchedBooks = new List<Books>();
            if (textSearched != null && textSearched.Length >= 3)
            {
                if (ListOfBooks != null && ListOfBooks.Count > 0)
                {
                    searchedBooks = ListOfBooks.Where(x => x.BookName.ToLower().Contains(textSearched)).ToList();

                    if (searchedBooks.Count > 0)
                    {
                        return searchedBooks;
                    }
                }
            }
            return searchedBooks;
        }
    }
}
