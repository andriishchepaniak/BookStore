using BookStoreGraphQLApi.GraphQL.Authors;
using BookStoreModels;
using BookStoreModels.GraphQLTypes.Authors;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreClient
{
    public record AuthorsResponse(List<Author> authors);

    public record AddAuthorResponse(AddAuthorPayload addAuthor);

    public class AuthorClientService
    {
        private readonly GraphQLHttpClient _client;
        public AuthorClientService()
        {
            _client = new GraphQLHttpClient("https://localhost:44367/graphql", new NewtonsoftJsonSerializer());
        }

        public async Task<List<Author>> GetAll()
        {
            var query = new GraphQLHttpRequest
            {
                Query = @"{
                    authors{
                        id
                        name
                        books{
                            name
                        }
                    }
                }
                "
            };

            var response = await _client.SendQueryAsync<AuthorsResponse>(query);
            return response.Data.authors;
        
        }

        public async Task<Author> AddAuthor(Author author)
        {
            var query = new GraphQLHttpRequest
            {
                Query = @"mutation AddAuthor($input: AddAuthorInput!){
                              addAuthor (input: $input) {
                                author {
                                  id
                                  name
                                }
                              }
                            }",
                OperationName = "AddAuthor",
                Variables = new { input = new { name = author.Name } }
            };

            var response = await _client.SendQueryAsync<AddAuthorResponse>(query);

            return response.Data.addAuthor.author;
        }

    }
}
