namespace dotnet8_user.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public ICollection<Products> Products { get; private set; }

        private Category() { }
        public Category(string name, string description, User user)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required.", nameof(description));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            Name = name;
            Description = description;
            IsActive = true;
            AssignUser(user);
        }

        public void AssignUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            User = user;
            UserId = user.Id;
        }
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name cannot be empty.", nameof(newName));

            Name = newName;
            UpdateDate();
        }
        public void UpdateDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
                throw new ArgumentException("Description cannot be empty.", nameof(newDescription));

            Description = newDescription;
            UpdateDate();
        }
    }
}
