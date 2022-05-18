using BookStoreModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class AuthorsRepository
    {
        private readonly BookStoreContext _db;
        public AuthorsRepository(BookStoreContext context)
        {
            _db = context;
        }

        public async Task<Author> Add(Author author)
        {
            var existAuthor = await _db.Authors.FirstOrDefaultAsync(a => a.Name == author.Name);

            if (existAuthor == null)
            {
                await _db.Authors.AddAsync(author);
                await _db.SaveChangesAsync();

                return await _db.Authors.FirstOrDefaultAsync(a => a.Name == author.Name);
            }

            return existAuthor;
        }
    }
}
