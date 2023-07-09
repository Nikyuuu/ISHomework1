using Microsoft.AspNetCore.Identity;
using OnlineCinema.Web.Models.Domain;

namespace OnlineCinema.Web.Models.Identity
{
    public class OnlineCinemaAppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}
