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

        // Admin - View All Users
        public async Task<IActionResult> Index()
            => View(await _context.Users.ToListAsync());

        // Admin - User - View Your Profile
        public async Task<IActionResult> Profile(long? id)
            => View(await _context.Users.FirstAsync(u => u.Id == id));

        // Admin - Delete One User
        public async Task<IActionResult> Delete(long id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();
            user.State = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // User - Edit your User
        public IActionResult Edit(long? id)
        {
            if (id == null)
                return NotFound();

            var user = _context.Users.Find(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        // User - Save edited User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            if (user == null)
                return NotFound();

            var userDb = _context.Users.Find(user.Id);

            if (userDb == null) 
                return NotFound();

            userDb.Name = user.Name;
            userDb.Email = user.Email;
            userDb.Password = user.Password;

            _context.Update(userDb);
            _context.SaveChanges();

            return RedirectToAction("Profile", new { id = userDb.Id });
        }
    }
}
