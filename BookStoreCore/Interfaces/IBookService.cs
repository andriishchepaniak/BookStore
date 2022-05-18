using BookStoreModels;
using BookStoreModels.GraphQLTypes.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore.Interfaces
{
    public interface IBookService
    {
        IQueryable<Book> Get();
        IQueryable<Book> GetByAuthorId(int authorId);
        Book GetById(int id);
        Task<AddBookPayload> Add(AddBookInput input);
    }
}
