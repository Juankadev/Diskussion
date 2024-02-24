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
    }
}
