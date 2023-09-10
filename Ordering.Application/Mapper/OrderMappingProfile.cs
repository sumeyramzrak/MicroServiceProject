using AutoMapper;
using Ordering.Application.Commands.OrderCreate;
using Ordering.Application.Responses;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mapper
{
    //Profil olarak tanıması için Profile sınıfından inherit almalı.
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderCreateCommand>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();

            //ReverseMap ile birlikte belirtilen iki türüde birbirine mapleyebileceğini belirtmiş oluyoruz.
        }
    }
}
