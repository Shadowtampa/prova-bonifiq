using FluentValidation;
using ProvaPub.Repository;
using ProvaPub.Requests;

namespace ProvaPub.Validators
{
    public class PlaceOrderRequestValidator : AbstractValidator<PlaceOrderRequest>
    {
        private readonly TestDbContext _dbContext;

        public PlaceOrderRequestValidator(TestDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(request => request.CustomerId)
                .Must(CustomerExists)
                .WithMessage("Cliente com ID {PropertyValue} não existe.");

            RuleFor(request => request.PaymentValue)
                .GreaterThan(0)
                .WithMessage("O valor do pagamento deve ser maior que zero.");

            RuleFor(request => request.PaymentMethod)
                .IsInEnum()
                .WithMessage("Método de pagamento inválido.");
        }

        private bool CustomerExists(int customerId)
        {
            return _dbContext.Customers.Any(c => c.Id == customerId);
        }
    }
}
