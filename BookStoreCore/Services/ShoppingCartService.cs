using BookStoreCore.Interfaces;
using BookStoreModels;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly BookStoreContext _context;

        public ShoppingCartService(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<Book> AddToCart(int bookId, int userId)
        {
            var user = await _context.Users
                .Include(u => u.ShoppingCart)
                    .ThenInclude(sc => sc.Books)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if(user.ShoppingCart == null)
            {
                user.ShoppingCart = new ShoppingCart
                {
                    Total = 0
                };
                user.ShoppingCart.Books = new List<Book>();
                await _context.SaveChangesAsync();
            }
            
            var dbBook = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            user.ShoppingCart.Books.Add(dbBook);
            user.ShoppingCart.Total = user.ShoppingCart.Books.Sum(book => book.Price);

            await _context.SaveChangesAsync();

            return dbBook;
        }

        public async Task<Book> DeleteFromCart(int bookId, int userId)
        {
            var user = await _context.Users
                .Include(u => u.ShoppingCart)
                    .ThenInclude(sc => sc.Books)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var dbBook = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            user.ShoppingCart.Books.Remove(dbBook);
            user.ShoppingCart.Total = user.ShoppingCart.Books.Sum(book => book.Price);

            await _context.SaveChangesAsync();

            return dbBook;
        }

        public async Task<ShoppingCart> GetByUserId(int userId)
        {
            var user = await _context.Users
                .Include(u => u.ShoppingCart.Books)
                    .ThenInclude(b => b.Author)
                .Include(u => u.ShoppingCart.Books)
                    .ThenInclude(b => b.Likes)
                .Include(u => u.ShoppingCart.Books)
                    .ThenInclude(b => b.DisLikes)
                .FirstOrDefaultAsync(u => u.Id == userId);


            if (user.ShoppingCart == null)
            {
                return null;
            }
            return user.ShoppingCart;
        }

        public User GetUser(int userId)
        {
            return _context.Users.SingleOrDefault(u => u.Id == userId);
        }
    }
}
