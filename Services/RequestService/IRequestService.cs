using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary1.Services.RequestService
{
    public interface IRequestService
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "", bool IsATneed = true, int timeout = 150);

        Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "");

        //Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "", Dictionary<string, string> Headers = null);
        Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "", Dictionary<string, string> Headers = null, bool isAccessTokenRequired = true, int timeout = 90, bool autoRetry = true);

        Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "");

        Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data, string token = "");

        Task<TResult> PostFileAsync<TResult>(string uri, Stream data, string fileName, Dictionary<string, string> headers = null);
        Task<bool> IsAccessTokenValid();
    }
}
