using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineCinema.Web.Data;
using OnlineCinema.Web.Models.Domain;
using OnlineCinema.Web.Models.DTO;

namespace OnlineCinema.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ADD: TicketsToCart
        public async Task<IActionResult> AddTicketToCart(Guid? id)
        {
            var ticket = await _context.Tickets.Where(t => t.Id.Equals(id)).FirstOrDefaultAsync();
            AddToShoppingCartDto model = new AddToShoppingCartDto
            {
                SelectedTicket = ticket,
                TicketId = ticket.Id,
                Quantity = 1
            };
            return View(model);
        }
        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicketToCart([Bind("TicketId", "Quantity")] AddToShoppingCartDto item)
        {  
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userShoppingCart = await _context.ShoppingCarts.Where(z => z.OwnerId.Equals(userId)).FirstOrDefaultAsync();

            if (item.TicketId != null && userShoppingCart != null)
            {
                var ticket = await _context.Tickets.Where(t => t.Id.Equals(item.TicketId)).FirstOrDefaultAsync();
                if (ticket != null)
                {
                    TicketInShoppingCart ticketToAdd = new TicketInShoppingCart
                    {
                        Ticket = ticket,
                        TicketId = ticket.Id,
                        ShoppingCart = userShoppingCart,
                        ShoppingCartId = userShoppingCart.Id,
                        Quantity = item.Quantity
                    };
                    _context.Add(ticketToAdd);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index", "Tickets");
            }
            return View(item);
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
              return _context.Tickets != null ? 
                          View(await _context.Tickets.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
        }

        [HttpPost]
        public async Task<IActionResult> Index(DateTime FilterDate)
        {
            List<Ticket> tiketi;

                tiketi = await _context.Tickets.Where(ticket => DateTime.Compare(ticket.DateTime, FilterDate.Date) < 0).ToListAsync();

            return View(tiketi);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketName,TicketDescription,TicketImage,TicketPrice,Rating,DateTime")] Ticket ticket)
        {

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    string errorMessage = error.ErrorMessage;
                    System.Diagnostics.Debug.WriteLine(errorMessage);
                    // Log or handle the error message
                }
            }

            if (ModelState.IsValid)
            {
                ticket.Id = Guid.NewGuid();
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TicketName,TicketDescription,TicketImage,TicketPrice,Rating,DateTime")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tickets'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
