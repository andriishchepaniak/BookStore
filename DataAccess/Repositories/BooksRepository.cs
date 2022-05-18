using BookStoreModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class BooksRepository
    {
        private readonly BookStoreContext _db;

        public BooksRepository(BookStoreContext context)
        {
            _db = context;
        }

        public async Task<List<Book>> GetAll()
        {
            return await _db.Books.ToListAsync();
        }

        public async Task<List<Book>> AddBooks(List<Book> books)
        {
            foreach (var book in books)
            {
                if (!_db.Books.Contains(book))
                {
                    await _db.Books.AddAsync(book);
                }
            }
            await _db.SaveChangesAsync();
            return books;
        }
        
        public async Task<Book> Add(Book book)
        {
            var existBook = await _db.Books.FirstOrDefaultAsync(b => b.Name == book.Name);

            if(existBook == null)
            {
                await _db.Books.AddAsync(book);
                await _db.SaveChangesAsync();

                return await _db.Books.FirstOrDefaultAsync(b => b.Name == book.Name);
            }

            return existBook;
        }
    }
}
