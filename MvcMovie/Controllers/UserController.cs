using Microsoft.AspNetCore.Mvc;
using Diskussion.Models;
using Diskussion.Models.ViewModels;
using Diskussion.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Diskussion.Utils;

namespace Diskussion.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IDiscussionRepository _discussionRepository;
        public UserController(IUserRepository userRepository,
            IDiscussionRepository discussionRepository)
        {
            _userRepository = userRepository;
            _discussionRepository = discussionRepository;
        }

        public IActionResult Index()
            => View(_userRepository.GetAll().ToList());

        public async Task<IActionResult> Profile(long? id)
        {
            if (id == null) return NotFound();

            var user = await _userRepository.GetById(id);

            if (user == null) return NotFound();

            ProfileViewModel profileVM = new ProfileViewModel();
            profileVM.User = user;
            profileVM.Discussions = await _discussionRepository.GetAllByIdWithResponses(id);
                 
            return View(profileVM);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            await _userRepository.HardDelete(id); //hacer soft delete
            await _userRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            User user = await _userRepository.GetById(id);

            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            if (!ModelState.IsValid) return View(user);

            var originalPassword = user.Password;
            user.Password = Encrypt.GETSHA256(user.Password);

            _userRepository.Update(user);
            await _userRepository.Save();

            HttpContext.Session.SetString("User_Name", user.Name);
            HttpContext.Session.SetString("User_Password", originalPassword);

            return RedirectToAction(nameof(Profile), new { id = user.Id });
        }

    }
}
