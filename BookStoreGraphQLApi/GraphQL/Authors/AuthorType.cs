using BookStoreCore.Interfaces;
using BookStoreModels;
using DataAccess;
using HotChocolate;
using HotChocolate.Types;
using System.Linq;

namespace BookStoreGraphQLApi.GraphQL.Authors
{
    public class AuthorType : ObjectType<Author>
    {
        protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
        {
            descriptor.Description("Represents model for author of book");

            descriptor
                .Field(a => a.Books)
                .ResolveWith<Resolvers>(a => a.GetBooks(default!, default!))
                .Description("This is the list of avaible books for this author");
        }

        public class Resolvers
        {
            public IQueryable<Book> GetBooks([Parent] Author author, [Service] IBookService bookService)
            {
                return bookService.GetByAuthorId(author.Id);
            }
        }
    }
}
