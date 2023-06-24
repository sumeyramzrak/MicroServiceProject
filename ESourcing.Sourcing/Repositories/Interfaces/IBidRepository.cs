using ESourcing.Sourcing.Entities;

namespace ESourcing.Sourcing.Repositories.Interfaces
{
    public interface IBidRepository
    {
        Task SendBid(Bid bid); //İlgili teklifleri kaydedeceğimiz metot.
        Task<List<Bid>> GetBidsByAuctionId(string id); //İhaleye gelen tüm teklifleri geri dönen metot.
        Task<List<Bid>> GetAllBidsByAuctionId(string id);
        Task<Bid> GetWinnerBid(string id); //İhalenin kazanan teklifini geri dönecek.
    }
}
