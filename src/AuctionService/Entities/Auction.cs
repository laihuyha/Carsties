using System;

namespace AuctionService.Entities
{
    public class Auction : BaseEntity
    {
        public Guid Id { get; set; }
        public int ReservePrice { get; set; }
        public string Seller { get; set; }
        public string Winner { get; set; }
        public int? SoldAmount { get; set; }
        public int? CurrentHighBid { get; set; }
        public DateTime AuctionEnd { get; set; }
        public Status Status { get; set; }
        public Item Item { get; set; }

        public bool HasReservePrice() => ReservePrice > 0;
    }
}