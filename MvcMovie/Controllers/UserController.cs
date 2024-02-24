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

        public async Task<IActionResult> Index(int id)
            => View(await _context.Users.FirstAsync(u=>u.Id==id));

        public async Task<IActionResult> List()
            => View(await _context.Users.ToListAsync());

        public IActionResult Delete(int id)
        {
            var user = _context.Users.First(u => u.Id == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
