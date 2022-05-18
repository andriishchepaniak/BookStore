using BookStoreCore.Interfaces;
using BookStoreModels;
using DataAccess;
using HotChocolate;
using HotChocolate.Types;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreGraphQLApi.GraphQL.Books
{
    public class BookType : ObjectType<Book>
    {
        protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
        {
            descriptor.Description("Represents book type for store");

            descriptor
                .Field(b => b.Author)
                .ResolveWith<Resolvers>(b => b.GetAuthor(default!, default!))
                .Description("");
        }

        private class Resolvers
        {
            public Author GetAuthor([Parent] Book book, [Service] IAuthorService authorService)
            {
                return authorService.GetById(book.AuthorId);
            }
        }
    }
}
