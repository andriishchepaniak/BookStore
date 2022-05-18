using BookStoreModels;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore.Interfaces
{
    public interface IPaymentService
    {
        Session CreateCheckoutSession(Order products);
    }
}
