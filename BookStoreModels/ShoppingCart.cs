using System.Collections.Generic;

namespace BookStoreModels
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public double Total { get; set; }
        public User User { get; set; }
        public List<Book> Books { get; set; }
    }
}