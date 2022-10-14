using BookLibrary1.Helpers;
using BookLibrary1.Model;
using BookLibrary1.Services;
using BookLibrary1.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace BookLibrary1.ViewModels
{
    public partial class CheckoutViewModel : ViewModelBase
    {
        //private readonly IFancyService service;

        public CheckoutViewModel()
        {
        }
        public async void Initialize(object shellFrame, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            try
            {
                ExpiryMinDate = new DateTimeOffset(DateTime.Now);
                ExpiryDate = new DateTimeOffset(DateTime.Now.AddDays(7));
                ExpiryMaxDate = new DateTimeOffset(DateTime.Now.AddDays(30));
            }
            catch (System.Exception ex)
            {
                LogError.TrackError(ex, "BookDetailsViewModel->Initialize");
            }
        }

        public event EventHandler? FormSubmissionCompleted;
        public event EventHandler? FormSubmissionFailed;


        private KeepType keepType;

        public KeepType KeepType
        {
            get { return keepType; }
            set { keepType = value; }
        }

        public ICommand SelectKeepTypeCmd => new RelayCommand<string>(SelectKeepType);
        public void SelectKeepType(string obj)
        {
            if (obj == "Rent")
            {
                KeepType = KeepType.Rent;
                IsExpiryVisible = Visibility.Visible;
            }
            else
            {
                KeepType = KeepType.Purchase;
                IsExpiryVisible = Visibility.Collapsed;
            }
        }

        #region FormFields
        private Visibility isExpiryVisible;

        public Visibility IsExpiryVisible
        {
            get { return isExpiryVisible; }
            set { isExpiryVisible = value; OnPropertyChanged(); }
        }

        private string firstName;


        public string FirstName
        {
            get { return firstName; }
            set
            {
                lastName = value; OnPropertyChanged();
                ValidateProperty("FirstName");
            }

        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; OnPropertyChanged(); }
        }

        private DateTimeOffset expiryDate;

        public DateTimeOffset ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; OnPropertyChanged(); }
        }

        private DateTimeOffset _ExpiryMinDate;

        public DateTimeOffset ExpiryMinDate
        {
            get { return _ExpiryMinDate; }
            set { _ExpiryMinDate = value; OnPropertyChanged(); }
        }

        private DateTimeOffset _ExpiryMaxDate;

        public DateTimeOffset ExpiryMaxDate
        {
            get { return _ExpiryMaxDate; }
            set { _ExpiryMaxDate = value; OnPropertyChanged(); }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }
        private string _PhoneNumber;

        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { _PhoneNumber = value; OnPropertyChanged(); }
        }
        private string _addressLine1;

        public string AddressLine1
        {
            get { return _addressLine1; }
            set { _addressLine1 = value; OnPropertyChanged(); }
        }

        private string _addressLine2;

        public string AddressLine2
        {
            get { return _addressLine2; }
            set { _addressLine2 = value; OnPropertyChanged(); }
        }

        private string _city;

        public string City
        {
            get { return _city; }
            set { _city = value; OnPropertyChanged(); }
        }
        private string _state;

        public string State
        {
            get { return _state; }
            set { _state = value; OnPropertyChanged(); }
        }

        private string _country;

        public string Country
        {
            get { return _country; }
            set { _country = value; OnPropertyChanged(); }
        }
        private string _pincode;

        public string PinCode
        {
            get { return _pincode; }
            set { _pincode = value; OnPropertyChanged(); }
        }
        #endregion
        public ICommand SubmitCommand => new RelayCommand(Submit);

        private async void Submit()
        {
            ValidateAllProperties();
            IsBusy = true;
            try
            {
                OrderDetails orderDetails = new OrderDetails
                {
                    UserID = AppSettings.Account.UserID,
                    BookID = AppSettings.BookID,
                    Amount = 450,
                    KeepType = KeepType,
                    Expiry = ExpiryDate,
                    FirstName = FirstName,
                    LastName= LastName,
                    Email= Email,
                    AddressLine1= AddressLine1,
                    AddressLine2= AddressLine2,
                    City= City,
                    State= State,
                    Country= Country,
                    PinCode=PinCode
                };
                AppSettings.OrderDetails = orderDetails;

                var response = await userServices.PostOrderDetails(orderDetails);
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
            }
            finally { IsBusy = false; }

        }

        public static ValidationResult ValidateName(string name, ValidationContext context)
        {
            CheckoutViewModel instance = (CheckoutViewModel)context.ObjectInstance;
            //bool isValid = instance.service.Validate(name);

            if (true)
            {
                return ValidationResult.Success;
            }

            return new("The name was not validated by the fancy service");
        }

        [RelayCommand]
        private void ShowErrors()
        {
            string message = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));

            //_ = DialogService.ShowMessageDialogAsync("Validation errors", message);
        }


    }
}
