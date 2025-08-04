using System.Text.Json.Serialization;

namespace ProvaPub.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentMethod
    {
        Pix,
        CreditCard,
        Paypal
    }

}