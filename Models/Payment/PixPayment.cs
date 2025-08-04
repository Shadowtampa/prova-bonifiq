namespace ProvaPub.Models.Payment
{
    public class PixPayment : PaymentMethod
    {
        private readonly HttpClient _httpClient;

        public PixPayment()
        {
            _httpClient = new HttpClient(); // Como vou fazer somente um overengineering, não vou fazer DI.
        }

        public override async Task<Order> ProcessPayment(decimal amount, int customerId)
        {
            Console.WriteLine("Processamento de pix");

            var payload = new
            {
                CustomerId = customerId,
                Amount = amount
            };

            var response = await _httpClient.PostAsJsonAsync("https://postman-echo.com/post", payload);

            if (response.IsSuccessStatusCode)
            {
                return new Order
                {
                    Value = amount,
                    CustomerId = customerId
                };
            }

            throw new Exception("Erro ao processar pagamento com Pix.");
        }
    }
}
