using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Diskussion.Models;

namespace Diskussion.Controllers
{
    public class UserController : Controller
    {
        private readonly DiskussionDbContext _context;

        public UserController(DiskussionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
            => View(await _context.Users.ToListAsync());

        public async Task<IActionResult> Profile(long id)
            => View(await _context.Users.FirstAsync(u => u.Id == id));

        public async Task<IActionResult> Delete(long id)
        {
            var user = _context.Users.Find(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
