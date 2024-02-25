using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Diskussion.Models;

namespace Diskussion.Controllers
{
    public class DiscussionController : Controller
    {
        private readonly DiskussionDbContext _context;

        public DiscussionController(DiskussionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var discussions = _context.Discussions.Include(d=>d.IdAuthorNavigation).OrderByDescending(d=>d.CreationDate);
            return View(await discussions.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Discussion discussion)
        {
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
            var newResponse = new Response()
            {
                IdAuthor = long.Parse(HttpContext.Session.GetString("User_Id")),
                IdDiscussion = response.IdDiscussion,
                Message = response.Message,
            };

            _context.Add(newResponse);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new {id= response.IdDiscussion});
        }

        public async Task<IActionResult> Discussion(long id)
        {
            Discussion discussion = await _context.Discussions
                .Include(d => d.IdAuthorNavigation) // Incluir al autor de la discusión
                .Include(d => d.Responses) // Incluir las respuestas de la discusión
                    .ThenInclude(r => r.IdAuthorNavigation) // Incluir al usuario asociado a cada respuesta
                .FirstAsync(d => d.Id == id);

            return View(discussion);
        }

        public async Task<IActionResult> Like(long id)
        {
            Response response = _context.Responses.Find(id);
            response.Likes++;
            _context.Update(response);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

