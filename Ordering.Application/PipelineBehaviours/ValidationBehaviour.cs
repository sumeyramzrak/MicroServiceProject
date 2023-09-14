
using FluentValidation;
using MediatR;

namespace Ordering.Application.PipelineBehaviours
{
    /*
     * ValidationBehaviour a mediator pattern üzerinden send metodu ile çalıştırdığımızda buraya istekte bulunacak.
     * İçerisine bir request ve işlemni tamamladıktan sonra next dediğmizde bir sonraki pipeline varsa praya geçmesini beklediğimizi belirttiğimiz bir delegate belirtiyoruz.
     Amacımız mediatorın içinden send metodu çalıştıktan sonra 
    her handler metoduna gelen request için validation işlemlerini kodlamak yerine
    ValidationBehaviour bunu bizim için yapacak. 
    Ve sıkıntı yoksa handlers klasörü altında handle metoduna gitmesine izin verecek.
    İşlemler tammalandıktan sonra return ile response u döndürecek.
     
     */
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators.Select(x => x.Validate(context))
                                      .SelectMany(x => x.Errors)
                                      .Where(x => x != null)
                                      .ToList();
            // failures ile hatalı olan validation değişkenleri döndürdük.
            if (failures.Any())
            {
                //Hatalı bir geri dönüş varsa validation hatası var demektir, exception döndür.
                throw new ValidationException(failures);
            }

            return next();
        }
    }
}
