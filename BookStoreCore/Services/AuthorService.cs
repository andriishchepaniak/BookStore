using BookStoreCore.Interfaces;
using BookStoreModels;
using BookStoreModels.GraphQLTypes.Authors;
using DataAccess;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStoreCore.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly BookStoreContext _context;

        public AuthorService(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<AddAuthorPayload> Add(AddAuthorInput input)
        {
            var existAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Name == input.name);

            if (existAuthor != null)
            {
                return new AddAuthorPayload(existAuthor);
            }

            var author = new Author
            {
                Name = input.name
            };

            var dbAuthor = _context.Authors.Add(author).Entity;

            await _context.SaveChangesAsync();

            return new AddAuthorPayload(dbAuthor);
        }

        public IQueryable<Author> Get()
        {
            return _context.Authors;
        }

        public Author GetById(int id)
        {
            return _context.Authors
                    .FirstOrDefault(a => a.Id == id);
        }
    }
}
