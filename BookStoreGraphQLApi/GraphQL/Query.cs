using BookStoreCore.Interfaces;
using BookStoreModels;
using DataAccess;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreGraphQLApi.GraphQL
{
    public class Query
    {
        [UseOffsetPaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Book> GetBooks([Service] IBookService bookService)
        {
            return bookService.Get();
        }

        public Book GetBookById(int id, [Service] IBookService bookService)
        {
            return bookService.GetById(id);
        }

        [UseOffsetPaging]
        //[UseFirstOrDefault]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Author> GetAuthors([Service] IAuthorService authorService)
        {
            return authorService.Get();
        }

        [UseProjection]
        public Author GetAuthor(int authorId, [Service] IAuthorService authorService)
        {
            return authorService.GetById(authorId);
        }
        //[Authorize]
        [UseProjection]
        public async Task<ShoppingCart> GetCart(int userId, [Service] IShoppingCartService cartService)
        {
            return await cartService.GetByUserId(userId);
        }

        [Authorize]
        public string Test()
        {
            return "is authenticated";
        }

        [UseProjection]
        public User GetUser(int userId, [Service] IShoppingCartService cartService)
        {
            return cartService.GetUser(userId);
        }

        //Orders
        public async Task<Order> GetOrderDetails(int orderId, [Service] IOrderService orderService)
        {
            return await orderService.GetOrderAsync(orderId);
        }

        public async Task<List<Order>> GetUserOrders(int userId, [Service] IOrderService orderService)
        {
            return await orderService.GetUserOrdersAsync(userId);
        }
    }
}
