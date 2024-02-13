using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketProject.Classes;

namespace TicketProject.Controllers
{
    [Route("api/Transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ShopContext _context;

        public TransactionsController(ShopContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transactions>>> Gettransaction()
        {
            return await _context.transaction.ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{userName}")]
        public async Task<ActionResult<IEnumerable<Transactions>>> GetTransactionsByUserName(string userName)
        {
            if (!int.TryParse(userName, out int userId))
            {
                return BadRequest("Nieprawidłowy format indeksu użytkownika.");
            }

            var transactions = await _context.transaction.Where(t => t.UserName == userId).ToListAsync();

            if (transactions == null || transactions.Count == 0)
            {
                return NotFound();
            }

            return transactions;
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactions(int id, Transactions transactions)
        {
            if (id != transactions.UserNameID)
            {
                return BadRequest();
            }

            _context.Entry(transactions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transactions>> PostTransactions(Transactions transactions)
        {
            _context.transaction.Add(transactions);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransactions", new { id = transactions.UserNameID }, transactions);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactions(int id)
        {
            var transactions = await _context.transaction.FindAsync(id);
            if (transactions == null)
            {
                return NotFound();
            }

            _context.transaction.Remove(transactions);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionsExists(int id)
        {
            return _context.transaction.Any(e => e.UserNameID == id);
        }
    }
}
