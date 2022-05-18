using BookStoreModels;
using BookStoreModels.GraphQLTypes.Authors;
using HotChocolate.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStoreCore.Interfaces
{
    public interface IAuthorService
    {
        IQueryable<Author> Get();
        Author GetById(int id);
        Task<AddAuthorPayload> Add(AddAuthorInput author);
    }
}
