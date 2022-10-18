using System;

namespace BookLibrary1.Model
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public AccountType? AccountType { get; set; }
    }

    public enum AccountType
    {
        Admin,
        User,
        Creator,
        Manager,
        Agent,
        Supervisior
    }
}
