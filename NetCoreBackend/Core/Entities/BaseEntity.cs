using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public abstract class BaseEntity : IEntity
    {
        public DateTime? Created { get; set; } = DateTimeOffset.UtcNow.UtcDateTime;
        public DateTime? Modified { get; set; } = DateTimeOffset.UtcNow.UtcDateTime;

    }
}