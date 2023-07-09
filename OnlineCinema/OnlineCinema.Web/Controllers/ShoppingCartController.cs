using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCinema.Web.Data;
using OnlineCinema.Web.Models.Domain;
using OnlineCinema.Web.Models.DTO;
using OnlineCinema.Web.Models.Identity;
using System.Security.Claims;

namespace OnlineCinema.Web.Controllers
{
    public class ShoppingCartController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInUser = await _context.Users.Where(t => t.Id == userId)
                .Include("ShoppingCart")
                .Include("ShoppingCart.TicketInShoppingCarts")
                .Include("ShoppingCart.TicketInShoppingCarts.Ticket")
                .FirstOrDefaultAsync();

            var userShoppingCart = loggedInUser.ShoppingCart;
            var allTickets = userShoppingCart.TicketInShoppingCarts.ToList();

            var allTicketPrice = allTickets.Select(t => new
            {
                ticketPrice = t.Ticket.TicketPrice,
                quantity = t.Quantity
            }).ToList();

            var totalPrice = 0;

            foreach (var ticket in allTicketPrice)
            {
                totalPrice += ticket.ticketPrice * ticket.quantity;
            }

            ShoppingCartDto scDto = new ShoppingCartDto
            {
                Tickets = allTickets,
                TotalPrice = totalPrice,
            };

            return View(scDto);
        }

        public async Task<IActionResult> DeleteFromShoppingCart(Guid id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!string.IsNullOrEmpty(userId) && id != null)
            {
                var loggedInUser = await _context.Users.Where(t => t.Id == userId)
                .Include("ShoppingCart")
                .Include("ShoppingCart.TicketInShoppingCarts")
                .Include("ShoppingCart.TicketInShoppingCarts.Ticket")
                .FirstOrDefaultAsync();

                var userShoppingCart = loggedInUser.ShoppingCart;

                var ticketToRemove = userShoppingCart.TicketInShoppingCarts.Where(t => t.TicketId.Equals(id)).FirstOrDefault();

                userShoppingCart.TicketInShoppingCarts.Remove(ticketToRemove);

                _context.Update(userShoppingCart);
                await _context.SaveChangesAsync();
            }
            
           return RedirectToAction("Index", "ShoppingCart");
        }

    }
}
