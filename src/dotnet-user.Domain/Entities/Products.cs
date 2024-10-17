namespace dotnet_user_api.Domain.Entities
{
    public class Products : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int QuantityInStock { get; private set; }
        public string ImageUrl { get; private set; }
        public string SKU { get; private set; }
        public bool IsActive { get; private set; }
        public Category Category { get; private set; }

        private Products() { }

        public Products(string name, string description, decimal price, int quantityInStock, string imageUrl, string sku, bool isActive)
        {
            Name = name;
            Description = description;
            Price = price;
            QuantityInStock = quantityInStock;
            ImageUrl = imageUrl;
            SKU = sku;
            IsActive = isActive;
        }
        public void UpdateStock(int newQuantity)
        {
            QuantityInStock = newQuantity;
        }
        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
