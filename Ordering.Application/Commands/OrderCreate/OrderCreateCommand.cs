using MediatR;
using Ordering.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Commands.OrderCreate
{
    /*
    IRequest responseun ne tipte olduğunu dynamic bir tipten istiyor.
    OrderCreateCommandı execute edecek handler OrderCreateCommand tipinde bir emir aldığında işlemini yapıp geri dönerken
    OrderResponse tipinde bir nesne dönmesi gerektiğini belirtiyoruz.
     */
    public class OrderCreateCommand : IRequest<OrderResponse>
    {
        //Bir Order create edeceğinden Order entitysinin propertylerini içeriyor.
        public string AuctionId { get; set; }
        public string SellerUserName { get; set; }
        public string ProductId { get; set; } 
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}


