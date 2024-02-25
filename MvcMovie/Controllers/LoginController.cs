using Diskussion.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diskussion.Controllers
{
    public class LoginController : Controller
    {
        private readonly DiskussionDbContext _context;

        public LoginController(DiskussionDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Verify(User user)
        {
            var userExist = _context.Users.FirstOrDefault(u => u.Name == user.Name && u.Password==user.Password && u.State == true);
            if(userExist!=null)
            {
                HttpContext.Session.SetString("User_Id", userExist.Id.ToString());
                HttpContext.Session.SetString("User_Name", userExist.Name);

                //dar los permisos necesarios en base al rol (agregar campo en la tabla)
                return RedirectToAction("Index", "Discussion");
            }

            return RedirectToAction("Index");
        }
    }
}
