using Diskussion.Models;
using Diskussion.Models.ViewModels;
using Diskussion.Repositories.Interfaces;
using Diskussion.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Diskussion.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel user)
        {
            if (!ModelState.IsValid) return View(user);

            var originalPassword = user.Password;
            user.Password = Encrypt.GETSHA256(user.Password);

            var userExist = _userRepository.GetAll(u =>
            u.Name == user.Name && u.Password == user.Password && u.State == true).ToList().FirstOrDefault();

            if (userExist == null)
                return View();

            HttpContext.Session.SetString("User_Id", userExist.Id.ToString());
            HttpContext.Session.SetString("User_Name", userExist.Name);
            HttpContext.Session.SetString("User_Password", originalPassword);
            //dar los permisos necesarios en base al rol (agregar campo en la tabla)
            return RedirectToAction("Index", "Discussion");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid) return View(user);

            var userExist = _userRepository.GetAll(u =>
            u.Name == user.Name || u.Email == user.Email).ToList().FirstOrDefault();

            if(userExist != null) return View(user);

            var originalPassword = user.Password;
            user.Password = Encrypt.GETSHA256(user.Password);

            var newUser = await _userRepository.Insert(user);
            await _userRepository.Save();

            HttpContext.Session.SetString("User_Id", newUser.Id.ToString());
            HttpContext.Session.SetString("User_Name", newUser.Name);
            HttpContext.Session.SetString("User_Password", originalPassword);
            //dar los permisos necesarios en base al rol (agregar campo en la tabla)
            return RedirectToAction("Index", "Discussion");
        }
    }
}
