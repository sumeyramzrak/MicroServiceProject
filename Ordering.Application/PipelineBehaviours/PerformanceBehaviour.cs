using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.PipelineBehaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehaviour(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {  
           
            //Behaviour içine girdikten sonra timer ı başlat.
            _timer.Start();
            //Delegate i execute ettik.
            var response = await next();
            //Delegatein geri dönüşüyle timer ı durdurduk.
            _timer.Stop();
            //geçen süreyi aldık.
            var elapsedMilliseconds = _timer.ElapsedMilliseconds; //Geçen süre
            //timer dan aldığımız süre bilgisine göre loglanıp loglanmaması gerektiğini belirledik.
            if (elapsedMilliseconds > 500)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", requestName, elapsedMilliseconds, request);
            }

            return response;
        }
    }
}
