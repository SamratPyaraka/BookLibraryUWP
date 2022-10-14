using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary1.Model
{
    public class UserRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UserResponse
    {
        public bool status { get; set; }
        public string responseMessage { get; set; }
    }

    public class APIResponse
    {
        public bool Response { get; set; }
        public string ResponseMessage { get; set; }

        public int Status { get; set; }
        public object Data { get; set; }
    }

    public class UserResponse<T>
    {
        public bool Response { get; set; }
        public string ResponseMessage { get; set; }

        public int Status { get; set; }
        public T Data { get; set; }
    }
}
