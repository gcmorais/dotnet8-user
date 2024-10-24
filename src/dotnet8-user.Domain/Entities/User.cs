﻿namespace dotnet8_user.Domain.Entities
{
    public sealed class User : BaseEntity
    {
        public string Fullname { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public byte[] HashPassword { get; private set; }
        public byte[] SaltPassword { get; private set; }
        public ICollection<Category> Categories { get; private set; }
        
        private User() { }
        public User(string fullname, string username, string email, byte[] hashPassword, byte[] saltPassword)
        {
            if (string.IsNullOrWhiteSpace(fullname))
                throw new ArgumentException("Name is required.", nameof(fullname));

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required.", nameof(username));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            Fullname = fullname;
            UserName = username;
            Email = email;
            HashPassword = hashPassword;
            SaltPassword = saltPassword;
        }

        public void UpdateName(string newFullname)
        {
            if (string.IsNullOrWhiteSpace(newFullname))
                throw new ArgumentException("Name cannot be empty.", nameof(newFullname));

            Fullname = newFullname;
            UpdateDate();
        }
        public void UpdateUsername(string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername))
                throw new ArgumentException("Username cannot be empty.", nameof(newUsername));

            UserName = newUsername;
            UpdateDate();
        }
        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentException("Email cannot be empty.", nameof(newEmail));

            Email = newEmail;
            UpdateDate();
        }

        public void AddCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (Categories == null)
                Categories = new HashSet<Category>();

            Categories.Add(category);
        }
        public void RemoveCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            Categories?.Remove(category);
        }
    }
}
