using System;

namespace AuctionService.Entities
{
    public class BaseEntity : GenericClassDeconstruct
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}