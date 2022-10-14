using BookLibrary1.Helpers;
using BookLibrary1.Model;
using BookLibrary1.Services;
using BookLibrary1.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Input;

namespace BookLibrary1.ViewModels
{
    public class BookDetailsViewModel : ViewModelBase
    {
        public BookDetailsViewModel()
        {
        }

        private List<Books> _Books;

        public List<Books> BookDetails
        {
            get { return _Books; }
            set { _Books = value; OnPropertyChanged(); }
        }

        public async void Initialize(object shellFrame, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            IsBusy = true;
            try
            {
                if (AppSettings.BookID > 0)
                {
                    BookDetails = await GetBooksList();
                }
            }
            catch (System.Exception ex)
            {
                LogError.TrackError(ex, "BookDetailsViewModel->Initialize");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<List<Books>> GetBooksList()
        {
            IsBusy = true;
            List<Books> books = new List<Books>();
            try
            {
                if (AppSettings.BookID > 0)
                {
                    var book = await userServices.GetBooks(AppSettings.BookID);
                    books.Add(book);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
            return books;
        }

        public ICommand RentOrPurchaseCmd => new RelayCommand(RentOrPurchase);
        public void RentOrPurchase()
        {
            IsBusy = true;
            OrderDetails orderDetails = new OrderDetails {
                UserID = AppSettings.Account.UserID,
                BookID = AppSettings.BookID,
                Amount = 450,
            };

            NavigationService.Navigate(typeof(CheckoutPage));
            IsBusy = false;
        }
    }
}
