using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineCinema.Web.Models.Domain;
using OnlineCinema.Web.Models.Identity;

namespace OnlineCinema.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<OnlineCinemaAppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<TicketInShoppingCart> TicketsInShoppingCart { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Ticket>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
               .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TicketInShoppingCart>()
                .HasKey(z => new { z.TicketId, z.ShoppingCartId });

            builder.Entity<TicketInShoppingCart>()
                .HasOne(z => z.Ticket)
                .WithMany(z => z.TicketInShoppingCarts)
                .HasForeignKey(z => z.TicketId);

            builder.Entity<TicketInShoppingCart>()
                .HasOne(z => z.ShoppingCart)
                .WithMany(z => z.TicketInShoppingCarts)
                .HasForeignKey(z => z.ShoppingCartId);

            builder.Entity<ShoppingCart>()
                .HasOne<OnlineCinemaAppUser>(z => z.Owner)
                .WithOne(z => z.ShoppingCart)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);
        }
    }
}