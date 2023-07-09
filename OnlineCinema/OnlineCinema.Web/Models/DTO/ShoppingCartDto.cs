using OnlineCinema.Web.Models.Domain;

namespace OnlineCinema.Web.Models.DTO
{
    public class ShoppingCartDto
    {
        public List<TicketInShoppingCart> Tickets { get; set; }
        public double TotalPrice { get; set; }
    }
}
