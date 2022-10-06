using BookLibrary1.Model;
using BookLibrary1.Services.RequestService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BookLibrary1.Services.UserService
{
    public class UserServices : IUserServices
    {
        private readonly IRequestService _requestService;

        public UserServices(IRequestService requestService)
        {
            _requestService = requestService;
        }
        public async Task<APIResponse> CreateUser(UserRequest ur)
        {
            APIResponse userInfo = new APIResponse();
            try
            {
                UriBuilder uriBuilder = new UriBuilder(AppSettings.CreateUserUri);
                string uri = uriBuilder.ToString();
                userInfo = await _requestService.PostAsync<UserRequest, APIResponse>(uri, ur);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userInfo;
        }

        public async Task<APIResponse> GetUserFromEmail(string email)
        {
            APIResponse userInfo = new APIResponse();
            try
            {
                UriBuilder uriBuilder = new UriBuilder(AppSettings.GetUserFromEmailUri);
                string uri = uriBuilder.ToString();
                userInfo = await _requestService.PostAsync<string, APIResponse>(uri, email);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userInfo;
        }

        public async Task<List<Books>> GetBooks()
        {
            List<Books> booksList = new List<Books>();
            try
            {
                UriBuilder uriBuilder = new UriBuilder(AppSettings.GetBooksUri);
                string uri = uriBuilder.ToString();
                booksList = await _requestService.GetAsync<List<Books>>(uri);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return booksList;
        }

        public async Task<Books> GetBooks(int bookID)
        {
            Books book = new Books();
            try
            {
                UriBuilder uriBuilder = new UriBuilder(AppSettings.GetBookUri);
                System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["bookID"] = bookID.ToString();
                uriBuilder.Query = query.ToString();
                string uri = uriBuilder.ToString();
                book = await _requestService.GetAsync<Books>(uri);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return book;
        }

        public async Task<APIResponse> UpdateBookDetails(Books books)
        {
            APIResponse bookUpdateDetails = new APIResponse();
            try
            {
                UriBuilder uriBuilder = new UriBuilder(AppSettings.UpdateBookDetailsUri);
                string uri = uriBuilder.ToString();
                bookUpdateDetails = await _requestService.PostAsync<Books, APIResponse>(uri, books);
                AppSettings.BookID = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bookUpdateDetails;
        }

        public async Task<APIResponse> UpdateBookStatus(int bookID)
        {
            APIResponse bookUpdateDetails = new APIResponse();
            try
            {
                UriBuilder uriBuilder = new UriBuilder(AppSettings.UpdateBookStatusUri);
                System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["bookID"] = bookID.ToString();
                uriBuilder.Query = query.ToString();
                string uri = uriBuilder.ToString();
                bookUpdateDetails = await _requestService.GetAsync<APIResponse>(uri);
                AppSettings.BookID = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bookUpdateDetails;
        }

        public async Task<APIResponse> CreateNewBook(Books books)
        {
            APIResponse bookUpdateDetails = new APIResponse();
            try
            {
                UriBuilder uriBuilder = new UriBuilder(AppSettings.CreateNewBookUri);
                string uri = uriBuilder.ToString();
                bookUpdateDetails = await _requestService.PostAsync<Books, APIResponse>(uri, books);
                AppSettings.BookID = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bookUpdateDetails;
        }
    }
}
