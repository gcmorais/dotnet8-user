using Microsoft.AspNetCore.Identity;

namespace dotnet_user_api.Infrastructure.Identity
{
    public class User : IdentityUser
    {
        public string FullName { get; private set; }
        public DateTime DateOfBirth { get; private set; }

        private User() { }

        public User(string fullName, DateTime dateOfBirth)
        {
            FullName = fullName;
            DateOfBirth = dateOfBirth;
        }
    }
}
