﻿using System;

using BookLibrary1.ViewModels;

using Windows.UI.Xaml.Controls;

namespace BookLibrary1.Views
{
    public sealed partial class UserAccountInfoPage : Page
    {
        public UserAccountInfoViewModel ViewModel { get; } = new UserAccountInfoViewModel();

        public UserAccountInfoPage()
        {
            InitializeComponent();
        }
    }
}
