using BookStoreModels;
using BookStoreModels.GraphQLTypes.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetOrderAsync(int id);
        Task<List<Order>> GetUserOrdersAsync(int userId);
        Task<Order> Create(AddOrderInput input);
        Task<Order> AddBooks(List<Book> books, int orderId);
        Task<Order> AddBook(Book book, int orderId);
    }
}
