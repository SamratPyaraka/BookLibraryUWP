using BookLibrary1.Model;
using BookLibrary1.Services;
using BookLibrary1.Services.UserService;
using BookLibrary1.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Input;

namespace BookLibrary1.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {

        private string _FirstNameText;

        public string FirstNameText
        {
            get { return _FirstNameText; }
            set { SetProperty(ref _FirstNameText, value); OnPropertyChanged(); }
        }

        private string _LastNameText;

        public string LastNameText
        {
            get { return _LastNameText; }
            set { SetProperty(ref _LastNameText, value); OnPropertyChanged(); }
        }

        private string _EmailText;

        public string EmailText
        {
            get { return _EmailText; }
            set { SetProperty(ref _EmailText, value); OnPropertyChanged(); }
        }

        private string _PasswordText;

        public string PasswordText
        {
            get { return _PasswordText; }
            set { SetProperty(ref _PasswordText, value); OnPropertyChanged(); }
        }

        private string _ConfirmPasswordText;


        public string ConfirmPasswordText
        {
            get { return _ConfirmPasswordText; }
            set { SetProperty(ref _ConfirmPasswordText, value); OnPropertyChanged(); }
        }

        public void Initialize(object shellFrame, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            FirstNameText = AppSettings.IDTokenPayLoad.GivenName;
            LastNameText = AppSettings.IDTokenPayLoad.FamilyName;
            EmailText = AppSettings.IDTokenPayLoad.Email;
        }

        public ICommand RegisterCMD => new RelayCommand(Register);

        private async void Register()
        {
            try
            {
                UserRequest userRequest = new UserRequest
                {
                    FirstName = AppSettings.IDTokenPayLoad.GivenName,
                    LastName = AppSettings.IDTokenPayLoad.FamilyName,
                    Email = AppSettings.IDTokenPayLoad.Email != null ? AppSettings.IDTokenPayLoad.Email: EmailText,
                    Password = EncodePasswordToBase64(PasswordText)
                };

                var response = await userServices.CreateUser(userRequest);
                if (response.Response)
                {

                    // Create the message dialog and set its content
                    await new MessageDialog(response.ResponseMessage, "").ShowAsync();
                    NavigationService.Navigate(typeof(ShellPage));
                }
                else
                {
                    // Create the message dialog and set its content
                    await new MessageDialog(response.ResponseMessage, "").ShowAsync();
                }
            }
            catch (Exception ex)
            {
            }
            
        }


        public string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
    }
}
