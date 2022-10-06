using BookLibrary1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary1.Services.UserService
{
    public interface IUserServices
    {
        Task<APIResponse> CreateUser(UserRequest ur);
        Task<APIResponse> GetUserFromEmail(string email);
        Task<List<Books>> GetBooks();
        Task<Books> GetBooks(int bookID);
        Task<APIResponse> UpdateBookDetails(Books books);
        Task<APIResponse> UpdateBookStatus(int bookID);
        Task<APIResponse> CreateNewBook(Books books);
    }
}
