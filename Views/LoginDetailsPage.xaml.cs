using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using BookLibrary1.Helpers;
using BookLibrary1.Model;
using BookLibrary1.Services;
using BookLibrary1.Services.UserService;
using BookLibrary1.ViewModels;
using Google.Apis.Auth;
using Windows.Data.Json;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BookLibrary1.Views
{
    public partial class  LoginDetailsPage : Page
    {

        const string clientID = "1076102788232-j2bbcig9jsbaqqvl4ns18l5everubm42.apps.googleusercontent.com";
        const string redirectURI = "uwp.books.library:/oaut2redirect";
        const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        const string tokenEndpoint = "https://www.googleapis.com/oauth2/v4/token";
        const string userInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";
        const string Scope = "https://www.googleapis.com/auth/userinfo.email";

        public LoginDetailsViewModel ViewModel { get; } = new LoginDetailsViewModel();

        public LoginDetailsPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, KeyboardAccelerators);

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (e.Parameter is Uri)
                {
                    // Gets URI from navigation parameters.
                    Uri authorizationResponse = (Uri)e.Parameter;
                    string queryString = authorizationResponse.Query;
                    output("LoginPage received authorizationResponse: " + authorizationResponse);

                    // Parses URI params into a dictionary
                    // ref: http://stackoverflow.com/a/11957114/72176
                    Dictionary<string, string> queryStringParams = queryString.Substring(1).Split('&')
                                                                    .ToDictionary(c => c.Split('=')[0],
                                                                    c => Uri.UnescapeDataString(c.Split('=')[1]));

                    if (queryStringParams.ContainsKey("error"))
                    {
                        output(String.Format("OAuth authorization error: {0}.", queryStringParams["error"]));
                        return;
                    }

                    if (!queryStringParams.ContainsKey("code")
                        || !queryStringParams.ContainsKey("state"))
                    {
                        output("Malformed authorization response. " + queryString);
                        return;
                    }

                    // Gets the Authorization code & state
                    string code = queryStringParams["code"];
                    string incoming_state = queryStringParams["state"];

                    // Retrieves the expected 'state' value from local settings (saved when the request was made).
                    ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                    string expected_state = (String)localSettings.Values["state"];

                    // Compares the receieved state to the expected value, to ensure that
                    // this app made the request which resulted in authorization
                    if (incoming_state != expected_state)
                    {
                        output(String.Format("Received request with invalid state ({0})", incoming_state));
                        return;
                    }

                    // Resets expected state value to avoid a replay attack.
                    localSettings.Values["state"] = null;

                    // Authorization Code is now ready to use!
                    output(Environment.NewLine + "Authorization code: " + code);

                    string code_verifier = (String)localSettings.Values["code_verifier"];
                    performCodeExchangeAsync(code, code_verifier);
                }
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "LoginDetailsPage->OnNavigatedTo");
            }

        }

        async void performCodeExchangeAsync(string code, string code_verifier)
        {
            try
            {
                string tokenRequestBody = string.Format("code={0}&redirect_uri={1}&client_id={2}&code_verifier={3}&scope=openid%20profile&grant_type=authorization_code",
                code,
                System.Uri.EscapeDataString(redirectURI),
                clientID,
                code_verifier
                );
                StringContent content = new StringContent(tokenRequestBody, Encoding.UTF8, "application/x-www-form-urlencoded");

                // Performs the authorization code exchange.
                HttpClientHandler handler = new HttpClientHandler();
                handler.AllowAutoRedirect = true;
                HttpClient client = new HttpClient(handler);

                output(Environment.NewLine + "Exchanging code for tokens...");
                HttpResponseMessage response = await client.PostAsync(tokenEndpoint, content);
                string responseString = await response.Content.ReadAsStringAsync();
                output(responseString);

                if (!response.IsSuccessStatusCode)
                {
                    output("Authorization code exchange failed.");
                    return;
                }

                // Sets the Authentication header of our HTTP client using the acquired access token.
                JsonObject tokens = JsonObject.Parse(responseString);
                AppSettings.GoogleAccessToken = tokens.GetNamedString("access_token");
                AppSettings.GoogleRefreshToken = tokens.GetNamedString("refresh_token");
                AppSettings.GAccessTokenID = tokens.GetNamedString("id_token");
                AppSettings.GAccessTokenExpiresIn = Convert.ToInt32(tokens.GetNamedNumber("expires_in"));

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AppSettings.GoogleAccessToken);

                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(tokens.GetNamedString("id_token"));
                output("JWT ID Token Payload Response..." + payload.ToString());
                var info = new IDTokenPayLoad()
                {
                    Scope = payload.Scope,
                    Email = payload?.Email,
                    EmailVerified = payload.EmailVerified,
                    FamilyName = payload.FamilyName,
                    GivenName = payload.GivenName,
                    HostedDomain = payload.HostedDomain,
                    Name = payload.Name,
                    Picture = payload.Picture

                };
                AppSettings.IDTokenPayLoad = info;

                if (!string.IsNullOrEmpty(AppSettings.IDTokenPayLoad?.Email))
                {
                    var userService = Locator.Instance.Resolve<IUserServices>();
                    var res = await userService.GetUserFromEmail(AppSettings.IDTokenPayLoad?.Email);
                    if (res.Response)
                    {
                        AppSettings.Account = res.Data;
                        NavigationService.Navigate(typeof(BooksStorePage));
                    }
                    else
                    {
                        NavigationService.Navigate(typeof(RegistrationPage));
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "LoginDetailsPage->performCodeExchangeAsync");
            }
            // Builds the Token request

        }

        /// <summary>
        /// Appends the given string to the on-screen log, and the debug console.
        /// </summary>
        public void output(string output)
        {
            try
            {
                textBoxOutput.Text = textBoxOutput.Text + output + Environment.NewLine;
                Debug.WriteLine(output);
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "LoginDetailsPage->output");
            }

        }

        /// <summary>
        /// Returns URI-safe data with a given input length.
        /// </summary>
        public string randomDataBase64url(uint length)
        {
            IBuffer buffer = CryptographicBuffer.GenerateRandom(length);
            return base64urlencodeNoPadding(buffer);
        }

        /// <summary>
        /// Returns the SHA256 hash of the input string.
        /// </summary>
        public IBuffer sha256(string inputString)
        {
            HashAlgorithmProvider sha = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(inputString, BinaryStringEncoding.Utf8);
            return sha.HashData(buff);
        }

        /// <summary>
        /// Base64url no-padding encodes the given input buffer.
        /// </summary>
        public string base64urlencodeNoPadding(IBuffer buffer)
        {
            string base64 = CryptographicBuffer.EncodeToBase64String(buffer);

            // Converts base64 to base64url.
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            // Strips padding.
            base64 = base64.Replace("=", "");

            return base64;
        }

        private void ConnectToGoogleCmd(object sender, RoutedEventArgs e)
        {
            try
            {
                // Generates state and PKCE values.
                string state = randomDataBase64url(32);
                string code_verifier = randomDataBase64url(32);
                string code_challenge = base64urlencodeNoPadding(sha256(code_verifier));
                const string code_challenge_method = "S256";

                // Stores the state and code_verifier values into local settings.
                // Member variables of this class may not be present when the app is resumed with the
                // authorization response, so LocalSettings can be used to persist any needed values.
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["state"] = state;
                localSettings.Values["code_verifier"] = code_verifier;

                // Creates the OAuth 2.0 authorization request.
                string authorizationRequest = string.Format("{0}?response_type=code&scope={6}&redirect_uri={1}&client_id={2}&state={3}&code_challenge={4}&code_challenge_method={5}&clientSecret={7}",
                    authorizationEndpoint,
                    System.Uri.EscapeDataString(redirectURI),
                    clientID,
                    state,
                    code_challenge,
                    code_challenge_method,
                    AppSettings.Scope+ " openid%20profile",
                    AppSettings.ClientSecret);

                output("Opening authorization request URI: " + authorizationRequest);

                // Opens the Authorization URI in the browser.
                var success = Windows.System.Launcher.LaunchUriAsync(new Uri(authorizationRequest));
            }
            catch (Exception ex)
            {
                LogError.TrackError(ex, "LoginDetailsPage->ConnectToGoogleCmd");
            }
        }
    }
}
