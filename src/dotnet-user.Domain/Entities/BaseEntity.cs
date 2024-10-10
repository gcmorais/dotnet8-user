namespace dotnet_user_api.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; private set; }
        public DateTimeOffset DateCreated { get; private set; }
        public DateTimeOffset? DateUpdated { get; private set; }
        public DateTimeOffset? DateDeleted { get; private set; }
        public bool IsDeleted => DateDeleted.HasValue;

        public BaseEntity()
        {
            Id = Guid.NewGuid();
            DateCreated = DateTimeOffset.UtcNow;
        }

        public void SetUpdated()
        {
            DateUpdated = DateTimeOffset.UtcNow;
        }

        public void SetDeleted()
        {
            DateDeleted = DateTimeOffset.UtcNow;
        }
    }

}
