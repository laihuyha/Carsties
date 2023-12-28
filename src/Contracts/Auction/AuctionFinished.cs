namespace Contracts.Auction
{
    public class AuctionFinished
    {
        public bool ItemSold { get; set; }
        public string AuctionId { get; set; }
        public string Winner { get; set; }
        public string Seller { get; set; }
        public string Amount { get; set; }
    }
}