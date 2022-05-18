using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreModels.GraphQLTypes.Orders
{
    public record AddOrderInput(double total, bool isPaid, int userId, List<Book> books);
}
