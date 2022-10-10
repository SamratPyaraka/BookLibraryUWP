using BookLibrary1.Helpers;
using BookLibrary1.Model;
using BookLibrary1.Services;
using BookLibrary1.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
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

        private Books _Books;

        public Books Books
        {
            get { return _Books; }
            set { _Books = value; OnPropertyChanged(); }
        }

        private string _BookName;

        public string BookName
        {
            get { return _BookName; }
            set {  _BookName= value; OnPropertyChanged(); }
        }

        private string _BookDescription;

        public string BookDescription
        {
            get { return _BookDescription; }
            set { _BookDescription = value; OnPropertyChanged(); }
        }

        private string _BookImageURL;

        public string BookImageURL
        {
            get { return _BookImageURL; }
            set { _BookImageURL = value; OnPropertyChanged(); }
        }

        private string editImage;

        public string EditImage
        {
            get { return editImage; }
            set { editImage = value; OnPropertyChanged(); }
        }

        private string deleteIcon;

        public string DeleteIcon
        {
            get { return deleteIcon; }
            set { deleteIcon = value; OnPropertyChanged(); }
        }

        private int bookCount;

        public int BookCount
        {
            get { return bookCount; }
            set { bookCount = value; OnPropertyChanged(); }
        }

        private string _BookCategory;

        public string BookCategory
        {
            get { return _BookCategory; }
            set { _BookCategory = value; OnPropertyChanged(); }
        }

        private int _KeepType;

        public int KeepType
        {
            get { return _KeepType; }
            set { _KeepType = value; OnPropertyChanged(); }
        }



        public async void Initialize(object shellFrame, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            try
            {
                if (AppSettings.BookID > 0)
                {
                    Books = await userServices.GetBooks(AppSettings.BookID);
                    BookName = Books.Title;
                    BookDescription = Books.Description;
                    BookImageURL = Books.ImageURL;
                    EditImage = AppSettings.DefaultEndpoint + "/BookImages/edit.png";
                    DeleteIcon = AppSettings.DefaultEndpoint + "/BookImages/delete.png";
                }
            }
            catch (System.Exception ex)
            {
                LogError.TrackError(ex, "BookDetailsViewModel->Initialize");
            }
        }

        public ICommand BookNameEdit => new RelayCommand(BookNameEditCMD);
        public ICommand DeleteBookCmd => new RelayCommand(DeleteBook);
        public ICommand CreateBookCmd => new RelayCommand(CreateBook);


        public async void CreateBook()
        {
            try
            {
                var newBook = new Books {
                    Title = BookName,
                    Description = BookDescription,
                    BookCount = BookCount,
                    KeepType = (KeepType)KeepType,
                    Category = BookCategory,
                    ImageURL = "/BookImages/TheKiteRunner.jpg"
                };

                var response = await userServices.CreateNewBook(newBook);
                if (response.Response)
                {
                    await new MessageDialog(response.ResponseMessage, "").ShowAsync();
                    NavigationService.Navigate(typeof(MainPage));
                }
                else
                {
                    await new MessageDialog(response.ResponseMessage, "").ShowAsync();
                }
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "BookDetailsViewModel->CreateBook");
            }
        }
        public async void DeleteBook()
        {
            try
            {
                Books.Title = BookName;
                var response = await userServices.UpdateBookStatus(AppSettings.BookID);
                if (response.Response)
                {
                    await new MessageDialog(response.ResponseMessage, "").ShowAsync();
                    NavigationService.Navigate(typeof(MainPage));
                }
                else
                {
                    await new MessageDialog(response.ResponseMessage, "").ShowAsync();
                }
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "BookDetailsViewModel->DeleteBook");
            }
        }
        public async void BookNameEditCMD()
        {
            try
            {
                Books.Title = BookName;
                var response = await userServices.UpdateBookDetails(Books);
                if (response.Response)
                {
                    await new MessageDialog(response.ResponseMessage, "").ShowAsync();
                    NavigationService.Navigate(typeof(MainPage));
                }
                else
                {
                    await new MessageDialog(response.ResponseMessage, "").ShowAsync();
                }
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "BookDetailsViewModel->BookNameEditCMD");
            }

        }

        public ICommand BookDescEdit => new RelayCommand(BookDescEditCMD);

        public async void BookDescEditCMD()
        {
            try
            {
                Books.Description = BookDescription;
                var response = await userServices.UpdateBookDetails(Books);
                if (response.Response)
                {
                    await new MessageDialog(response.ResponseMessage, "").ShowAsync();
                    NavigationService.Navigate(typeof(MainPage));
                }
                else
                {
                    await new MessageDialog(response.ResponseMessage, "").ShowAsync();
                }
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "BookDetailsViewModel->BookDescEditCMD");
            }
        }

    }
}
