using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Diskussion.Models;
using Diskussion.Models.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Diskussion.Repositories.Interfaces;

namespace Diskussion.Controllers
{
    public class DiscussionController : Controller
    {
        private readonly IDiscussionRepository _discussionRepository;

        public DiscussionController(IDiscussionRepository discussionRepository)
        {
            _discussionRepository = discussionRepository;
        }

        public IActionResult Index(string title)
        {
            return View(_discussionRepository.GetAllByTitle(title));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiscussionViewModel discussion)
        {
            if (!ModelState.IsValid) return View(discussion);
            if (HttpContext.Session.GetString("User_Id") == null) return View(discussion);

            var newDiscussion = new Discussion()
            {
                IdAuthor = long.Parse(HttpContext.Session.GetString("User_Id")),
                Title = discussion.Title,
                Description = discussion.Description
            };

            await _discussionRepository.Insert(newDiscussion);        
            await _discussionRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Open(long id)
        {
            return View(_discussionRepository.GetByIdWithIncludes(id));
        }

        public async Task<IActionResult> Like(long id)
        {
            //Response response = _context.Responses.Find(id);
            //response.Likes++;
            //_context.Update(response);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var discussion = await _discussionRepository.GetById(id);

            if (discussion == null) return NotFound();

            await _discussionRepository.HardDelete(id);
            await _discussionRepository.Save();

            return RedirectToAction("Profile", nameof(User), new { id = discussion.IdAuthor });
        }

        //public async Task<IActionResult> DeleteResponse(long? id)
        //{
        //    if (id == null)
        //        return NotFound();

        //    var response = _context.Responses.Find(id);

        //    if (response == null)
        //        return NotFound();

        //    response.State = false;
        //    _context.Responses.Update(response);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Discussion), new { id = response.IdDiscussion });
        //}
    }
}

