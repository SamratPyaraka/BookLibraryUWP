using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using BookLibrary1.Helpers;
using BookLibrary1.Model;
using BookLibrary1.Services;
using BookLibrary1.Views;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using Windows.UI.Xaml.Input;

namespace BookLibrary1.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {

        }

        private IncrementalLoadingCollection<BookSource, Books> _listOfBooks;

        public IncrementalLoadingCollection<BookSource, Books> ListOfBooks
        {
            get { return _listOfBooks; }
            set { SetProperty(ref _listOfBooks, value); OnPropertyChanged(); }
        }

        public IncrementalLoadingCollection<BookSource, Books> BooksList { get; set; }

        private Books _OnBookSelected;

        public Books OnBookSelected
        {
            get { return _OnBookSelected; }
            set { _OnBookSelected = value; OnPropertyChanged(); }
        }

        private string _TextSearched;

        public string TextSearched
        {
            get { return _TextSearched; }
            set { _TextSearched = value; OnPropertyChanged(); }
        }

        public IncrementalLoadingCollection<BookSource, Books> GetBooksList()
        {
            return ListOfBooks;
        }

        public void Initialize(object shellFrame, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            IsBusy = true;
            try
            {
                AppSettings.BookID = 0;
                ListOfBooks = new IncrementalLoadingCollection<BookSource, Books>(AppSettings.LoadItemsOffset);
                //ListOfBooks = new IncrementalLoadingCollection<BookSource, Books>(20, GetBooks);
            }
            catch (System.Exception ex)
            {
                LogError.TrackError(ex, "MainViewModel->Initialize");
            }
            finally { IsBusy = false; }
        }

        public ICommand CreateBookCmd => new RelayCommand(CreateBook);
        public void CreateBook()
        {
            NavigationService.Navigate(typeof(BookDetailsPage));
        }

        private async void GetBooks()
        {
            var listOfBooks = await userServices.GetLimitedBooks(0, AppSettings.LoadItemsOffset);
            foreach(var book in listOfBooks)
            {
                ListOfBooks.Add(book);
            }
        }

        public class BookSource : ViewModelBase, IIncrementalSource<Books>
        {
            private List<Books> ListOfBooks;
            public BookSource()
            {
                ListOfBooks = new List<Books>();
            }
            public async Task<IEnumerable<Books>> GetPagedItemsAsync(int pageIndex, int pageSize,
                CancellationToken cancellationToken = default)
            {
                IsBusy= true;
                ListOfBooks = await userServices.GetLimitedBooks(pageIndex * pageSize, pageSize);
                // Gets items from the collection according to pageIndex and pageSize parameters.
                var result = (from p in ListOfBooks select p);
                IsBusy= false;  
                return result;
            }
        }
    }
}
