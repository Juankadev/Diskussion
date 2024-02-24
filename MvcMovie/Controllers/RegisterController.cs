using Diskussion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diskussion.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DiskussionDbContext _context;

        public RegisterController(DiskussionDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
