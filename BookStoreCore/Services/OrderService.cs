using BookStoreCore.Interfaces;
using BookStoreModels;
using BookStoreModels.GraphQLTypes.Orders;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly BookStoreContext _context;

        public OrderService(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<Order> AddBook(Book book, int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Books)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return null;
            }

            
            var dbBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            order.Books.Add(dbBook);

            order.Total = order.Books.Sum(b => b.Price);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> AddBooks(List<Book> books, int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Books)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if(order == null)
            {
                return null;
            }

            foreach (var book in books)
            {
                var dbBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
                order.Books.Add(dbBook);
            }

            order.Total = order.Books.Sum(b => b.Price);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> Create(AddOrderInput input)
        {
            var order = new Order
            {
                Total = input.total,
                CreatedDate = DateTime.Now,
                IsPaid = input.isPaid,
                UserId = input.userId
            };

            var dbOrder = (await _context.Orders.AddAsync(order)).Entity;
            await _context.SaveChangesAsync();

            //var newOrder = await AddBooks(input.books, dbOrder.Id);

            return dbOrder;
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Books)
                    .ThenInclude(b => b.Author)
                .Include(o => o.Books)
                    .ThenInclude(b => b.Likes)
                .Include(o => o.Books)
                    .ThenInclude(b => b.DisLikes)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Books)
                    .ThenInclude(b => b.Author)
                .Include(o => o.Books)
                    .ThenInclude(b => b.Likes)
                .Include(o => o.Books)
                    .ThenInclude(b => b.DisLikes)
                .ToListAsync();
        }
    }
}
