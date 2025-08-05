using FluentValidation;
using ProvaPub.Repository;
using ProvaPub.Requests;

namespace ProvaPub.Validators
{
    public class CanPurchaseRequestValidator : AbstractValidator<CanPurchaseRequest>
    {
        private readonly TestDbContext _dbContext;

        protected virtual DateTime GetCustomNow() => DateTime.UtcNow;


        public CanPurchaseRequestValidator(TestDbContext dbContext)
        {
            _dbContext = dbContext;

            // Já tem teste
            RuleFor(request => request.CustomerId)
                .GreaterThan(0)
                .WithMessage("O valor do ID deve ser maior que zero.");

            // Já tem teste
            RuleFor(request => request.CustomerId)
                .Must(CustomerExists)
                .WithMessage("Cliente com ID {PropertyValue} não existe.");

            // Já tem teste
            RuleFor(request => request.PurchaseValue)
                .GreaterThan(0)
                .WithMessage("O valor da compra deve ser maior que zero.");

            // Já tem teste
            RuleFor(request => request.CustomerId)
                .Must(CostumerCanPurchaseOnlyOnceMonth)
                .WithMessage("O cliente já efetuou uma compra este mês.");

            // Já tem teste
            RuleFor(request => request)
                .Must(CustomerFirstPurchaseMaximum)
                .WithMessage("A primeira compra de um cliente é de no máximo 100");

            // Já tem teste
            RuleFor(request => request)
                .Must(PurchasesOnlyBusinessHoursWorkingDays)
                .WithMessage("As compras só são permitidas durante dias da semana e hora útil");

        }
        private bool CustomerExists(int customerId)
        {
            return _dbContext.Customers.Any(c => c.Id == customerId);
        }

        private bool CostumerCanPurchaseOnlyOnceMonth(int customerId)
        {
            var baseDate = DateTime.UtcNow.AddMonths(-1);
            var ordersInThisMonth = _dbContext.Orders
                .Count(s => s.CustomerId == customerId && s.OrderDate >= baseDate);

            return ordersInThisMonth == 0;
        }

        private bool CustomerFirstPurchaseMaximum(CanPurchaseRequest request)
        {
            var haveBoughtBefore = _dbContext.Customers.Count(s => s.Id == request.CustomerId && s.Orders.Any());
            return !(haveBoughtBefore == 0 && request.PurchaseValue > 100);
        }

        private bool PurchasesOnlyBusinessHoursWorkingDays(CanPurchaseRequest request)
        {
            var now = GetCustomNow();
            return !(now.Hour < 8 || now.Hour > 18 || now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday);
        }
    }
}
