using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using System.Diagnostics;
using BookLibrary1.Views;
using Google.Apis.Auth;
using Newtonsoft.Json.Linq;

namespace BookLibrary1.Services.RequestService
{
    public class RequestService : IRequestService
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public RequestService()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };

            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string token = "", bool IsATneed = true, int timeout = 150)
        {
#if DEBUG
            Debug.WriteLine($"--Start Of Get Call----");
#endif
            HttpClient httpClient = await CreateHttpClient(token, isAccessTokenRequired: IsATneed, timeout: timeout);

#if DEBUG
            Debug.WriteLine($"RQ_RS Request@@ {DateTime.Now} @@Url {uri}");
#endif
            CancellationTokenSource timeoutSource = new CancellationTokenSource(timeout * 1000);
            HttpResponseMessage response = await httpClient.GetAsync(uri, timeoutSource.Token);

            await HandleResponse(response);

            string serialized = await response.Content.ReadAsStringAsync();

#if DEBUG
            Debug.WriteLine($"RQ_RS Response@@ {DateTime.Now} @@Url {uri} @@Data {serialized}");
#endif
            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
#if DEBUG
            Debug.WriteLine($"--End Of Get Call----");
#endif
            return result;
        }

        public Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "")
        {
            return PostAsync<TResult, TResult>(uri, data, token);
        }

        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "", Dictionary<string, string> Headers = null, bool isAccessTokenRequired = true, int timeout = 90, bool autoRetry = true)
        {

            HttpClient httpClient = await CreateHttpClient(token, Headers, isAccessTokenRequired, timeout, autoRetry: autoRetry);
            string serialized = await Task.Run(() => JsonConvert.SerializeObject(data, _serializerSettings));

#if DEBUG
            Debug.WriteLine($"RQ_RS Request@@ {DateTime.Now} @@Url {uri} @@Request {serialized}");
#endif

            HttpResponseMessage response = await httpClient.PostAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            string responseData = await response.Content.ReadAsStringAsync();

#if DEBUG
            Debug.WriteLine($"RQ_RS Response@@ {DateTime.Now} @@Url {uri} @@Response {responseData}");
#endif

            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, _serializerSettings));
        }

        public Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "")
        {
            return PutAsync<TResult, TResult>(uri, data, token);
        }

        public async Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data, string token = "")
        {
            HttpClient httpClient = await CreateHttpClient(token);
            string serialized = await Task.Run(() => JsonConvert.SerializeObject(data, _serializerSettings));
            HttpResponseMessage response = await httpClient.PutAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            string responseData = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, _serializerSettings));
        }

        public async Task<TResult> PostFileAsync<TResult>(string uri, Stream data, string fileName, Dictionary<string, string> Headers = null)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            HttpContent fileStreamContent = new StreamContent(data);

            fileStreamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(420);
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(fileStreamContent);
                    if (Headers != null)
                        foreach (var item in Headers)
                        {
                            client.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                        }

                    response = await client.PostAsync(uri, formData);
                    await HandleResponse(response);
                    string responseData = await response.Content.ReadAsStringAsync();
                    return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, _serializerSettings));
                }
            }
        }

        public async Task<bool> IsAccessTokenValid()
        {
            try
            {
                if (!string.IsNullOrEmpty(AppSettings.GAccessTokenID))
                {
                    GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(AppSettings.GAccessTokenID);

                    if (payload != null)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                if(!string.IsNullOrEmpty(ex.Message) && ex.Message.ToLower().Contains("expired"))
                {
                    AppSettings.GAccessTokenID = "";
                }
            }
            return false;
        }

        HttpClient httpClient = null;

        private async Task<HttpClient> CreateHttpClient(string token = "", Dictionary<string, string> Headers = null, bool isAccessTokenRequired = true, int timeout = 90, bool autoRetry = true)
        {
            Headers = new Dictionary<string, string>();
            Headers.Add("AppPackageName", AppInfo.Current.PackageFamilyName);
            Headers.Add("AppVersion", "1");

            if (!await IsAccessTokenValid())
            {
                NavigationService.Navigate(typeof(LoginDetailsPage));
            }
            if (httpClient != null)
                return httpClient;

            httpClient = new HttpClient();

            httpClient.Timeout = TimeSpan.FromSeconds(150);
            if (Headers != null)
                foreach (var item in Headers)
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                }

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;


        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception(content);
                }

                throw new HttpRequestException(content, new Exception(response.RequestMessage.RequestUri.ToString()));
            }
        }


    }
}
