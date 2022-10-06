using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BookLibrary1.Model;
using BookLibrary1.Services;
using BookLibrary1.Views;
using CommunityToolkit.Mvvm.Input;
using Windows.UI.Xaml.Input;

namespace BookLibrary1.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            
        }

        private List<Books> _listOfBooks;

        public List<Books> ListOfBooks
        {
            get { return _listOfBooks; }
            set { _listOfBooks = value; OnPropertyChanged();  }
        }

        private Books _OnBookSelected;

        public Books OnBookSelected
        {
            get { return _OnBookSelected; }
            set { _OnBookSelected = value; OnPropertyChanged(); }
        }


        public async void Initialize(object shellFrame, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            try
            {
                AppSettings.BookID = 0;
                ListOfBooks = await userServices.GetBooks();
            }
            catch (System.Exception ex)
            {

            }
           
        }

        public ICommand CreateBookCmd => new RelayCommand(CreateBook);
        public async void CreateBook()
        {
            NavigationService.Navigate(typeof(BookDetailsPage));
        }

        //public ICommand OnBookClicked => new RelayCommand(OnBookClick);

        //public async void OnBookClick(int index)
        //{
        //    AppSettings.BookID = ListOfBooks[index];
        //    //NavigationService.Navigate(typeof(BookDetailsPage));
        //}

    }
}
