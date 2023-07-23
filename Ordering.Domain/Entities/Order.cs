using Ordering.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities
{
    public class Order : Entity
    {
        public string AuctionId { get; set; } //Hangi ihaleden siparişe dönüştü
        public string SellerUserName { get; set; } //ihaleyi kazanan kişi
        public string ProductId { get; set; } //hangi ürün için teklif kazandı
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
