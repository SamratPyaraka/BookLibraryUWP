using System;

using BookLibrary1.ViewModels;

using Windows.UI.Xaml.Controls;

namespace BookLibrary1.Views
{
    public sealed partial class BlankPage : Page
    {
        public BlankViewModel ViewModel { get; } = new BlankViewModel();

        public BlankPage()
        {
            InitializeComponent();
        }
    }
}
