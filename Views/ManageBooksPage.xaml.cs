using System;

using BookLibrary1.ViewModels;

using Windows.UI.Xaml.Controls;

namespace BookLibrary1.Views
{
    public sealed partial class ManageBooksPage : Page
    {
        public ManageBooksViewModel ViewModel { get; } = new ManageBooksViewModel();

        public ManageBooksPage()
        {
            InitializeComponent();
        }
    }
}
