using System;

using BookLibrary1.ViewModels;

using Windows.UI.Xaml.Controls;

namespace BookLibrary1.Views
{
    public sealed partial class ManageUsersPage : Page
    {
        public ManageUsersViewModel ViewModel { get; } = new ManageUsersViewModel();

        public ManageUsersPage()
        {
            InitializeComponent();
        }
    }
}
