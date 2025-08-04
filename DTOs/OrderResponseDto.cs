using ProvaPub.Models;

namespace ProvaPub.DTO
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }

        public static OrderResponseDto FromDomain(Order order, Customer customer)
        {
            return new OrderResponseDto()
            {
                Id = order.Id,
                Value = order.Value,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate.AddHours(-3),
                Customer = customer
            };
        }


    }

}