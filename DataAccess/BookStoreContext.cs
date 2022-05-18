using BookStoreModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BookStoreContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<DisLike> DisLikes { get; set; }

        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Books relations
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Orders)
                .WithMany(o => o.Books);
            
            modelBuilder.Entity<Book>()
                .HasMany(b => b.ShoppingCarts)
                .WithMany(o => o.Books);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Likes)
                .WithOne(l => l.Book)
                .HasForeignKey(l => l.BookId);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.DisLikes)
                .WithOne(d => d.Book)
                .HasForeignKey(d => d.BookId);

            //Authors relations
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId);

            //Users relations
            modelBuilder.Entity<User>()
                .HasOne(u => u.ShoppingCart)
                .WithOne(c => c.User)
                .HasForeignKey<User>(u => u.ShoppingCartId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Likes)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.DisLikes)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId);


            //Orders relations
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Books)
                .WithMany(b => b.Orders);

            //Shopping cart relations
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.User)
                .WithOne(u => u.ShoppingCart)
                .HasForeignKey<User>(u => u.ShoppingCartId);

            modelBuilder.Entity<ShoppingCart>()
                .HasMany(sc => sc.Books)
                .WithMany(b => b.ShoppingCarts);

            //Likes relations
            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Likes)
                .HasForeignKey(l => l.BookId);

            //DisLikes relations
            modelBuilder.Entity<DisLike>()
                .HasOne(d => d.User)
                .WithMany(u => u.DisLikes)
                .HasForeignKey(d => d.UserId);

            modelBuilder.Entity<DisLike>()
                .HasOne(d => d.Book)
                .WithMany(d => d.DisLikes)
                .HasForeignKey(d => d.BookId);
        }
    }
}
