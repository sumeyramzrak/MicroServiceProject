using AutoMapper;
using EventBusRabbitMQ.Events;
using Ordering.Application.Commands.OrderCreate;

namespace ESourcing.Order.Mapping
{
    //Profil olarak tanıması için Profile sınıfından inherit almalı.
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<OrderCreateEvent, OrderCreateCommand>().ReverseMap();
            //ReverseMap ile birlikte belirtilen iki türüde birbirine mapleyebileceğini belirtmiş oluyoruz.
        }
    }
}
