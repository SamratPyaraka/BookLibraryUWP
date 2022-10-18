using BookLibrary1.Model;
using System;


namespace BookLibrary1.ViewModels
{
    public class BooksStoreViewModel : ViewModelBase
    {
        public BooksStoreViewModel()
        {
        }

        private Books _OnBookSelected;

        public Books OnBookSelected
        {
            get { return _OnBookSelected; }
            set { _OnBookSelected = value; OnPropertyChanged(); }
        }
    }
}
