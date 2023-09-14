using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.PipelineBehaviours
{
    /*
     Mediator üzerinden send metodunu çalıştırdığımızda arka tarafta çalışan bloğuo try-catch içinde enkapsule ederek
     herhangi bir yönetilemeyen hata durumunda kontrollü bir şekilde yönetilmesini ve 
     nasıl bir işlem yapılması gerekiyorsa devam edebilmesini sağlayacak.
     - exception fırlattırmayabiliriz.
     - kontrollü bir şekilde response verip hataya düşmemesini sağlayabiliriz.
     - loglayabiliriz.
     - throw fırlatabiliriz ve yönetilmeyen bir handle/exceptionı yönetilmeyen bir kod olduğunda log tarafında incelenerek gerekli geliştirmelerin yapılmasını sağlayabiliriz.
     */
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            /*
            Burada çalışan kodlar herhangi bir hata alırsa ve execute edildiği yerde bu hata yönetilmiyorsa bunu 
            exception fırlarıldığında bunu bizim handlerımız yakalayacak. Ve istersek responseu manipule edip bir response dönmesini sağlayacağız
            ya da throw fırlatmasına devam edicez ve bunu loglucaz.
             */
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, "CleanArchitecture Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

                throw;
            }
        }
    }
}
