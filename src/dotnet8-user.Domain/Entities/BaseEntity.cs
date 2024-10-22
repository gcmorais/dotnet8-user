using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet8_user.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public DateTimeOffset? DateDeleted { get; set; }

        public BaseEntity()
        {
            Id = Guid.NewGuid();
            DateCreated = DateTimeOffset.UtcNow;
        }
        public void UpdateDate()
        {
            DateUpdated = DateTimeOffset.UtcNow;
        }
        public void MarkAsDeleted()
        {
            DateDeleted = DateTimeOffset.UtcNow;
        }
    }
}
