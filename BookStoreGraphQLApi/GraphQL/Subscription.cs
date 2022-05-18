using BookStoreModels;
using HotChocolate;
using HotChocolate.Types;

namespace BookStoreGraphQLApi.GraphQL
{
    public class Subscription
    {
        [Subscribe]
        [Topic]
        public Author OnAuthorAdded([EventMessage] Author author)
        {
            return author;
        }
    }
}
