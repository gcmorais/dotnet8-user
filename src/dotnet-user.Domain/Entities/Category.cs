namespace dotnet_user_api.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; }
        public ICollection<Products> Products { get; private set; }

        private Category() { }

        public Category(string name, string userId)
        {
            Name = name;
            Products = new List<Products>();
        }

        public void UpdateName(string newName)
        {
            Name = newName;
        }
    }
}
