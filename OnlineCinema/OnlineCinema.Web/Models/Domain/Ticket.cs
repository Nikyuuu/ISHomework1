using System.ComponentModel.DataAnnotations;

namespace OnlineCinema.Web.Models.Domain
{
    public class Ticket
    {
        public Guid Id { get; set; }
        [Required]
        public string TicketName { get; set; }
        [Required]
        public string TicketDescription { get; set; }
        [Required]
        public string TicketImage { get; set; }
        [Required]
        public int TicketPrice { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public DateTime DateTime { get; set; }

        public virtual ICollection<TicketInShoppingCart>? TicketInShoppingCarts { get; set; }

        public Ticket()
        {
            DateTime now = DateTime.Now;
            DateTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        }
    }
}
