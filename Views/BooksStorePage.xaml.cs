using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BookLibrary1.Helpers;
using BookLibrary1.Model;
using BookLibrary1.Services;
using BookLibrary1.Services.UserService;
using BookLibrary1.ViewModels;
using Microsoft.Toolkit.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using static BookLibrary1.ViewModels.MainViewModel;

namespace BookLibrary1.Views
{
    public sealed partial class BooksStorePage : Page
    {
        bool incall = false, endoflist = false;
        int offSet = 0;
        ObservableCollection<Books> booksList = new ObservableCollection<Books>();
        public BooksStoreViewModel ViewModel { get; } = new BooksStoreViewModel();

        public IUserServices userServices = Locator.Instance.Resolve<IUserServices>();

        public BooksStorePage()
        {
            InitializeComponent();
            LoadData(++offSet);
        }

        private void gridView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ScrollViewer scrollViewer = GetScrollViewer(this.gridSource);
            scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            double progress = scrollViewer.VerticalOffset / scrollViewer.ScrollableHeight;
            Debug.WriteLine("Progress :" + progress);
            if (progress > AppSettings.ScrollVerticalOffset && !incall && !endoflist)
            {
                incall = true;
                progressRing.IsActive = true;
                LoadData(++offSet);

            }
        }
        public async Task LoadData(int offset)
        {
            try
            {
                int start = offSet * AppSettings.LoadItemsOffset;
                var bListTobeLoaded = await userServices.GetLimitedBooks(start, start + AppSettings.LoadItemsOffset);
                var count = bListTobeLoaded.Count;
                if (count > 0)
                {
                    foreach (var book in bListTobeLoaded)
                    {
                        booksList.Add(book);
                    }
                }
                else
                {
                    endoflist = true;
                }
                progressRing.IsActive = false;
                incall = false;
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "LoadData");
            }

        }

        private void gridSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int selectedIndex = gridSource.SelectedIndex;
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

        public static ScrollViewer GetScrollViewer(DependencyObject dObj)
        {
            if (dObj is ScrollViewer) return dObj as ScrollViewer;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dObj); i++)
            {
                var child = VisualTreeHelper.GetChild(dObj, i);
                var result = GetScrollViewer(child);
                if (result != null) return result;
            }
            return null;
        }

        private void gridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int selectedIndex = gridSource.SelectedIndex;
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
        private async  void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            try
            {
                if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    if (textSearched.Text != null & textSearched.Text.Length >=3)
                    {

                        var books =await PopulateSearchedBooks(textSearched.Text);
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

        private async void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            try
            {
                if (!string.IsNullOrEmpty(args.QueryText))
                {
                    //Do a fuzzy search based on the text
                    var suggestions = await PopulateSearchedBooks(sender.Text);
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
        public async Task<List<Books>> PopulateSearchedBooks(string textSearched)
        {
            List<Books> searchedBooks = new List<Books>();
            try
            {
                if (booksList != null && booksList.Count > 0)
                {
                    searchedBooks = booksList.Where(x => x.Title.ToLower().Contains(textSearched)).ToList();

                    if (searchedBooks.Count > 0)
                    {
                        return searchedBooks;
                    }
                    else
                    {
                        if (textSearched.Length >= 3)
                        {
                            searchedBooks = await userServices.GetBooks(textSearched);
                            if (searchedBooks.Count > 0)
                            {
                                return searchedBooks;
                            }
                        }
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
