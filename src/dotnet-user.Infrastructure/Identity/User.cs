using dotnet_user_api.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace dotnet_user_api.Infrastructure.Identity
{
    public class User : IdentityUser
    {
        public string FullName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public ICollection<Category> Categories { get; private set; }

        private User()
        {
            Categories = new List<Category>();
        }
        public User(string userName, string email, string fullName, DateTime dateOfBirth) : this()
        {
            UserName = userName;
            Email = email;
            FullName = fullName;
            DateOfBirth = dateOfBirth;
        }

        public void UpdateFullName(string newFullName)
        {
            FullName = newFullName;
        }
        public void AddCategory(Category category)
        {
            Categories.Add(category);
        }
    }
}
