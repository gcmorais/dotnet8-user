namespace dotnet8_user.Domain.Entities
{
    public class Products : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }
        public string Brand { get; private set; }
        public string ImageUrl { get; private set; }
        public bool IsActive { get; private set; }
        public Category Category { get; private set; }
        public Guid CategoryId { get; private set; }

        private Products() { }
        public Products(string name, string description, decimal price, int stockquantity, string brand, string imageurl, Category category)
        {
            Validations(name, description, price, stockquantity, brand, imageurl);

            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockquantity;
            Brand = brand;
            ImageUrl = imageurl;
            IsActive = true;
            AssignCategory(category);
        }
        public void AssignCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            Category = category;
            CategoryId = category.Id;
        }

        public void Validations(string name, string description, decimal price, int stockquantity, string brand, string imageurl)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required", nameof(description));

            if (price < 0)
                throw new ArgumentException("The price cannot be negative", nameof(price));

            if (stockquantity < 0)
                throw new ArgumentException("The stock quantity cannot be negative", nameof(stockquantity));

            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Brand is required", nameof(brand));

            if (string.IsNullOrWhiteSpace(imageurl))
                throw new ArgumentException("Image url is required", nameof(imageurl));
        }
    }
}
