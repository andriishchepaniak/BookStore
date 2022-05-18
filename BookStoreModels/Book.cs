using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreModels
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public List<Order> Orders { get; set; }
        public List<ShoppingCart> ShoppingCarts { get; set; }
        public List<Like> Likes { get; set; }
        public List<DisLike> DisLikes { get; set; }
    }
}
