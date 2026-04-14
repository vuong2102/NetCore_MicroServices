using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Domain
{
    public abstract class BaseModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
