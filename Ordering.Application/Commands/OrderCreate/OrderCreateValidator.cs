using FluentValidation;

namespace Ordering.Application.Commands.OrderCreate
{
    /// <summary>
    /// Commandın execute edilemeden önce validate olmasını sağlayacak.
    /// </summary>

    public class OrderCreateValidator :AbstractValidator<OrderCreateCommand>
    {
        /*
        * Fluent validatora tanıtabilmek için AbstractValidatordan türetmemiz gerekiyor.
        * OrderCreateValidator ın hangi nesne üzerinden validate yapacağını AbstractValidator içerisinde belirtiyoruz
        * Validationları constructor metot içerisinde tanımlıyoruz.
        */
        public OrderCreateValidator()
        {
            RuleFor(v => v.SellerUserName).EmailAddress().NotEmpty(); //Nokta ile ard arda ilgili metodu çağırma --> fluent api design pattern
            RuleFor(v => v.ProductId).NotEmpty();
        }
    }
}
