using BookStoreCore.Interfaces;
using BookStoreModels;
using BookStoreModels.GraphQLTypes.Books;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore.Services
{
    public class BookService : IBookService
    {
        private readonly BookStoreContext _context;

        public BookService(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<AddBookPayload> Add(AddBookInput input)
        {
            var existBook = await _context.Books.FirstOrDefaultAsync(b => b.Name == input.name);

            if (existBook != null)
            {
                return new AddBookPayload(existBook);
            }

            var book = new Book
            {
                Name = input.name,
                Price = input.price,
                Description = input.description,
                ImageUrl = input.imageUrl,
                AuthorId = input.authorId
            };

            var dbBook = _context.Books.Add(book).Entity;

            await _context.SaveChangesAsync();

            return new AddBookPayload(dbBook);
        }

        public IQueryable<Book> Get()
        {
            return _context.Books;
        }

        public IQueryable<Book> GetByAuthorId(int authorId)
        {
            return _context.Books
                .Where(book => book.AuthorId == authorId);
        }

        public Book GetById(int id)
        {
            return _context.Books
                .Include(book => book.Author)
                .FirstOrDefault(book => book.Id == id);
        }
    }
}
