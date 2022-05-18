using BookStoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetByUserId(int userId);
        Task<Book> AddToCart(int bookId, int userId);
        Task<Book> DeleteFromCart(int bookId, int userId);
        User GetUser(int userId);
    }
}
