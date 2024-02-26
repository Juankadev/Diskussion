using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Diskussion.Models;
using Diskussion.Models.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Diskussion.Controllers
{
    public class DiscussionController : Controller
    {
        private readonly DiskussionDbContext _context;

        public DiscussionController(DiskussionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string title)
        {
            IQueryable<Discussion> discussions = _context.Discussions.Where(d=>d.State==true).Include(d => d.IdAuthorNavigation).Include(d => d.Responses).OrderByDescending(d => d.CreationDate);

            if (!string.IsNullOrEmpty(title))
                discussions = discussions.Where(d => d.Title.Contains(title));

            var list = await discussions.ToListAsync();
            var newList = new List<Discussion>();

            foreach (var dis in list)
            {
                dis.Responses = dis.Responses.Where(r=>r.State==true).ToList();
                newList.Add(dis);
            }

            return View(newList);
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

            var newDiscussion = new Discussion()
            {
                IdAuthor = long.Parse(HttpContext.Session.GetString("User_Id")),
                Title = discussion.Title,
                Description = discussion.Description
            };
            _context.Add(newDiscussion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateResponse(Response response)
        {
            if(string.IsNullOrEmpty(response.Message)) 
                return RedirectToAction(nameof(Discussion), new { id = response.IdDiscussion });

            var newResponse = new Response()
            {
                IdAuthor = long.Parse(HttpContext.Session.GetString("User_Id")),
                IdDiscussion = response.IdDiscussion,
                Message = response.Message,
            };

            _context.Add(newResponse);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Discussion), new {id= response.IdDiscussion});
        }

        public async Task<IActionResult> Discussion(long id)
        {
            Discussion discussion = await _context.Discussions
                .Include(d => d.IdAuthorNavigation) // Incluir al autor de la discusión
                .Include(d => d.Responses) // Incluir las respuestas de la discusión
                    .ThenInclude(r => r.IdAuthorNavigation) // Incluir al usuario asociado a cada respuesta
                .FirstAsync(d => d.Id == id);

            discussion.Responses = discussion.Responses.Where(r => r.State == true).ToList();

            return View(discussion);
        }

        public async Task<IActionResult> Like(long id)
        {
            Response response = _context.Responses.Find(id);
            response.Likes++;
            _context.Update(response);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteResponse(long? id)
        {
            if (id == null)
                return NotFound();

            var response = _context.Responses.Find(id);

            if (response == null)
                return NotFound();

            response.State = false;
            _context.Responses.Update(response);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Discussion), new { id = response.IdDiscussion });
        }
    }
}

