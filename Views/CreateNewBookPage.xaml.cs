using BookLibrary1.ViewModels;

using Windows.UI.Xaml.Controls;

namespace BookLibrary1.Views
{
    public sealed partial class CreateNewBookPage : Page
    {
        public CreateNewBookViewModel ViewModel { get; } = new CreateNewBookViewModel();

        public CreateNewBookPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, KeyboardAccelerators);
        }

        private void Image_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.BookNameEditCMD();
        }

        private void Image_Tapped_1(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.BookDescEditCMD();
        }
        private void DeleteIconTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.DeleteBook();
        }
    }
}
