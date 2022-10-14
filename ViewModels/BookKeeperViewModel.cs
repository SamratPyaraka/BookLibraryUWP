using BookLibrary1.Helpers;
using BookLibrary1.Model;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Input;

namespace BookLibrary1.ViewModels
{
    public class BookKeeperViewModel : ViewModelBase
    {
        public BookKeeperViewModel()
        {
        }

        private List<BookRecords> bookList;

        public List<BookRecords> BookList
        {
            get { return bookList; }
            set { bookList = value; OnPropertyChanged(); }
        }

        public async void Initialize(object shellFrame, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            IsBusy = true;
            try
            {
                BookList = await userServices.GetBooksByUserID(AppSettings.Account.UserID);
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "BookKeeperViewModel->Initialize");
            }
            finally { IsBusy = false; }
        }

    }
}
