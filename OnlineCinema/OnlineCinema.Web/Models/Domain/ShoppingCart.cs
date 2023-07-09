using OnlineCinema.Web.Models.Identity;

namespace OnlineCinema.Web.Models.Domain
{
    public class ShoppingCart 
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; }
        public OnlineCinemaAppUser Owner { get; set; }
        public virtual ICollection<TicketInShoppingCart> TicketInShoppingCarts { get; set; }
    }
}
