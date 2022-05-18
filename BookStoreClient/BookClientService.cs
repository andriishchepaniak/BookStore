using BookStoreGraphQLApi.GraphQL.Books;
using BookStoreModels;
using BookStoreModels.GraphQLTypes.Books;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreClient
{
    public record BooksResponse(List<Book> books);

    public record AddBookResponse(AddBookPayload addBook);
    public class BookClientService
    {
        private readonly GraphQLHttpClient _client;

        public BookClientService()
        {
            _client = new GraphQLHttpClient("https://localhost:44367/graphql", new NewtonsoftJsonSerializer());
        }

        public async Task<List<Book>> GetBooks()
        {
            var query = new GraphQLHttpRequest
            {
                Query = @"books {
                            name
                            author {
                              id
                              name
                            }
                          }"
            };

            var response = await _client.SendQueryAsync<BooksResponse>(query);

            return response.Data.books;
        }

        public async Task<Book> AddBook(Book book)
        {
            var query = new GraphQLHttpRequest
            {
                Query = @"mutation AddBook($input: AddBookInput!){
                              addBook (input: $input) {
                                book {
                                  id
                                  name
                                  price
                                  description
                                  imageUrl
                                  author {
                                    id
                                    name
                                  }
                                }
                              }
                            }",
                OperationName = "AddBook",
                Variables = new
                {
                    input = new
                    {
                        name = book.Name,
                        price = book.Price,
                        description = book.Description,
                        imageUrl = book.ImageUrl,
                        authorId = book.AuthorId
                    }
                }
            };

            var response = await _client.SendQueryAsync<AddBookResponse>(query);

            return response.Data.addBook.book;
        }
    }
}
