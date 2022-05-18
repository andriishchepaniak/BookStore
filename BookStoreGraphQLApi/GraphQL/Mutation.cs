using BookStoreCore.Authentication;
using BookStoreCore.Interfaces;
using BookStoreGraphQLApi.GraphQL.Authors;
using BookStoreGraphQLApi.GraphQL.Books;
using BookStoreModels;
using BookStoreModels.AuthenticationModels;
using BookStoreModels.GraphQLTypes.Authors;
using BookStoreModels.GraphQLTypes.Books;
using BookStoreModels.GraphQLTypes.Dislikes;
using BookStoreModels.GraphQLTypes.Likes;
using BookStoreModels.GraphQLTypes.Orders;
using DataAccess;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStoreGraphQLApi.GraphQL
{
    public class Mutation
    {
        //Authors
        public async Task<AddAuthorPayload> AddAuthor(AddAuthorInput input, [Service] IAuthorService authorService)
        {
            return await authorService.Add(input);
        }

        //Books
        public async Task<AddBookPayload> AddBook(AddBookInput input, 
            [Service] IBookService bookService)
        {
            return await bookService.Add(input);
        }

        //Authentication and register
        public async Task<User> Register(RegisterModel model, [Service] IAuthService authService)
        {
            return await authService.Register(model);
        }
        
        public async Task<AuthResponse> Login(LoginModel model, [Service] IAuthService authService)
        {
            return await authService.Login(model);
        }

        //Likes
        [Authorize]
        public async Task<AddLikePayload> AddLike(AddLikeInput input, [Service] ILikeService likeService)
        {
            return await likeService.Add(input);
        }

        [Authorize]
        public async Task<Like> RemoveLike(int likeId, [Service] ILikeService likeService)
        {
            return await likeService.Remove(likeId);
        }

        //DisLikes
        [Authorize]
        public async Task<DisLike> AddDisLike(AddDisLikeInput input, [Service] IDisLikeService disLikeService)
        {
            return await disLikeService.Add(input);
        }

        [Authorize]
        public async Task<DisLike> RemoveDisLike(int disLikeId, [Service] IDisLikeService disLikeService)
        {
            return await disLikeService.Remove(disLikeId);
        }

        //Shopping cart
        public async Task<Book> AddToCart(int bookId, int userId, [Service] IShoppingCartService cartService)
        {
            return await cartService.AddToCart(bookId, userId);
        }

        public async Task<Book> DeleteFromCart(int bookId, int userId, [Service] IShoppingCartService cartService)
        {
            return await cartService.DeleteFromCart(bookId, userId);
        }

        //Orders
        public async Task<Order> CreateOrder(AddOrderInput input, [Service] IOrderService orderService)
        {
            return await orderService.Create(input);
        }

        public async Task<Order> AddBooksToOrder(List<Book> books, int orderId, [Service] IOrderService orderService)
        {
            return await orderService.AddBooks(books, orderId);
        }
        public async Task<Order> AddBookToOrder(Book book, int orderId, [Service] IOrderService orderService)
        {
            return await orderService.AddBook(book, orderId);
        }

        //Payment
        public string CreateCheckoutSession(Order order, [Service] IPaymentService paymentService)
        {
            var result = paymentService.CreateCheckoutSession(order);

            return result.Url;
        }
    }
}
