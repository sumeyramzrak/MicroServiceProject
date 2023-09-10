using AutoMapper;
using MediatR;
using Ordering.Application.Commands.OrderCreate;
using Ordering.Application.Responses;
using Ordering.Domain.Entities;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Handlers
{
    /// <summary>
    /// input olarak OrderCreateCommand, response olarak OrderResponse
    /// </summary>
    public class OrderCreateHandler : IRequestHandler<OrderCreateCommand, OrderResponse>
    {
        /*
         * OrderCreateHandler meditor library kullanan metot üzerinden belirtilen request tipinde request aldığında bu handler çalışacak.
         * Handle metot altındaki logic işleyecek
         * Geri dönüşte de OrderResponse tipinde dönecek.
         */
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderCreateHandler(IOrderRepository orderRepository,IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderResponse> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
        {
            //Öncelikle orderEntity i oluşturmamız lazım yani IOrderRepository üzerinden bir insert işlemi yapıcaz.
            //Handle metoduna gelen OrderCreateCommand ı orderEntity e dönüştürüyoruz.(nesneler birebir maplenebildiği için ekstra bir manipulasyon yapmıyoruz)
            var orderEntity = _mapper.Map<Order>(request);

            //Validationlar
            if (orderEntity==null) 
                throw new ApplicationException("Entity couldnt be mapped!");

            //OrderRepository yardımıyla insert işlemi yapacak metodu sayesinde oluşturduğumuz entityi veritabanına gönderdik.
            var order =  await _orderRepository.AddAsync(orderEntity);

            //AddAsync metodu bize order tipinde bir nesne dönüyor. 
            //Oluşan order tipindeki nesneyi Mapper kullanarak order nesnemizi OrderResponse a dönüştürüyoruz
            var orderResponse =_mapper.Map<OrderResponse>(order);

            return orderResponse;

        }
    }
}
