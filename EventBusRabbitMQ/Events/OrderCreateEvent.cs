using EventBusRabbitMQ.Events.Interfaces;

namespace EventBusRabbitMQ.Events
{
    public class OrderCreateEvent : IEvent
    {
        //Bırakılacak event bir kazanan teklif olacağından Bid clasının bütün parametrelerini içermeli.
        public string Id { get; set; }
        public string AuctionId { get; set; }
        public string ProductId { get; set; }
        public string SellerUserName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        //Ek olarak Auction miktarının da eventte bulunmasını istiyoruz.
        public int Quantity { get; set; }
    }
}
