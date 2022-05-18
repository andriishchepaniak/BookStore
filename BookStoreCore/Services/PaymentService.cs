using BookStoreCore.Interfaces;
using BookStoreModels;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderService _orderService;

        public PaymentService(IOrderService orderService)
        {
            Stripe.StripeConfiguration.ApiKey = "sk_test_51KnIhUEzJmkmdvkXtW0en9DhiMfEsOg8hWmlV1FKJvx70ZNo5uU9AgDA4y7XqC6XrhuwC7xpoB2jFgjun4Ljy68I00i7ZL9Wm8";
            _orderService = orderService;
        }
        public Session CreateCheckoutSession(Order order)
        {
            var lineItems = new List<SessionLineItemOptions>();

            foreach (var book in order.Books)
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = Convert.ToDecimal(book.Price * 100),
                        Currency = "uah",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = book.Name,
                            Images = new List<string> { book.ImageUrl}
                        }
                    },
                    Quantity = 1
                });
            }
            var options = new SessionCreateOptions
            {
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = $"http://localhost:3000/main/orders",
                CancelUrl = "http://localhost:18473/cart",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }
    }
}
