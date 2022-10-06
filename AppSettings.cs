using Plugin.Settings.Abstractions;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using BookLibrary1.Model;

namespace BookLibrary1
{
    public class AppSettings
    {

        private static readonly AppSettings _instance = new AppSettings();
        private static string _baseEndpointPrimary;

        private static string _baseEndpoint;

#if RELEASE
        public const string DefaultEndpoint = "https://api.flexsalary.com/APIV1";  
        public static string ReleaseMode = "Production";
#else
        public static string DefaultEndpoint = "http://localhost:5221";
        public static string ReleaseMode = "QA";
#endif
        private static ISettings Settings => CrossSettings.Current;

        public static AppSettings Instance => _instance;

        public static string CreateUserUri => $"{DefaultEndpoint}/api/Users";
        public static string GetUserFromEmailUri => $"{DefaultEndpoint}/api/Users/GetUserFromEmail";
        public static string GetBooksUri => $"{DefaultEndpoint}/api/Books";
        public static string GetBookUri => $"{DefaultEndpoint}/api/Books/GetBooks";
        public static string UpdateBookDetailsUri => $"{DefaultEndpoint}/api/Books/UpdateBookDetails";
        public static string UpdateBookStatusUri => $"{DefaultEndpoint}/api/Books/UpdateBookStatus";
        public static string CreateNewBookUri => $"{DefaultEndpoint}/api/Books/CreateNewBook";



        public static string GoogleAccessToken
        {
            get => Settings.GetValueOrDefault(nameof(GoogleAccessToken), string.Empty);
            set => Settings.AddOrUpdateValue(nameof(GoogleAccessToken), value);
        }

        public static string GoogleRefreshToken
        {
            get => Settings.GetValueOrDefault(nameof(GoogleRefreshToken), string.Empty);
            set => Settings.AddOrUpdateValue(nameof(GoogleRefreshToken), value);
        }

        public static string GEmail
        {
            get => Settings.GetValueOrDefault(nameof(GEmail), string.Empty);
            set => Settings.AddOrUpdateValue(nameof(GEmail), value);
        }

        public static string GFamilyName
        {
            get => Settings.GetValueOrDefault(nameof(GFamilyName), string.Empty);
            set => Settings.AddOrUpdateValue(nameof(GFamilyName), value);
        }

        public static string GProfilePic
        {
            get => Settings.GetValueOrDefault(nameof(GProfilePic), string.Empty);
            set => Settings.AddOrUpdateValue(nameof(GProfilePic), value);
        }

        public static string GGivenName
        {
            get => Settings.GetValueOrDefault(nameof(GGivenName), string.Empty);
            set => Settings.AddOrUpdateValue(nameof(GGivenName), value);
        }

        public static string GName
        {
            get => Settings.GetValueOrDefault(nameof(GName), string.Empty);
            set => Settings.AddOrUpdateValue(nameof(GName), value);
        }

        public static int GAccessTokenExpiresIn
        {
            get => Settings.GetValueOrDefault(nameof(GAccessTokenExpiresIn), 0);
            set => Settings.AddOrUpdateValue(nameof(GAccessTokenExpiresIn), value);
        }

        public static string GAccessTokenType
        {
            get => Settings.GetValueOrDefault(nameof(GAccessTokenType), string.Empty);
            set => Settings.AddOrUpdateValue(nameof(GAccessTokenType), value);
        }

        public static string GAccessTokenID
        {
            get => Settings.GetValueOrDefault(nameof(GAccessTokenID), string.Empty);
            set => Settings.AddOrUpdateValue(nameof(GAccessTokenID), value);
        }
        public static string GAccessTokenScope
        {
            get => Settings.GetValueOrDefault(nameof(GAccessTokenScope), string.Empty);
            set => Settings.AddOrUpdateValue(nameof(GAccessTokenScope), value);
        }

        public static IDTokenPayLoad IDTokenPayLoad
        {
            get => Settings.GetValueOrDefault(nameof(IDTokenPayLoad), default(IDTokenPayLoad));
            set => Settings.AddOrUpdateValue(nameof(IDTokenPayLoad), value);
        }

        public static int BookID
        {
            get => Settings.GetValueOrDefault(nameof(BookID), 0);
            set => Settings.AddOrUpdateValue(nameof(BookID), value);
        }

    }

}
