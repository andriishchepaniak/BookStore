namespace BookStoreModels.GraphQLTypes.Books
{
    public record AddBookInput(string name, double price, string description, string imageUrl, int authorId);
}
