using System;
using System.Collections.Generic;

namespace BookStoreModels
{
    public class Order
    {
        public int Id { get; set; }
        public double Total { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsPaid { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Book> Books { get; set; }
    }
}