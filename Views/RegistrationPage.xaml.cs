using System;
using Autofac;
using BookLibrary1.Services.UserService;
using BookLibrary1.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace BookLibrary1.Views
{
    public sealed partial class RegistrationPage : Page
    {
        
        public RegistrationViewModel ViewModel { get; } = new RegistrationViewModel();
        public RegistrationPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame,  KeyboardAccelerators);
        }
    }
}
