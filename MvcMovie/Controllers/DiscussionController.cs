using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Diskussion.Models;
using Diskussion.Models.ViewModels;

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
            IQueryable<Discussion> discussions = _context.Discussions.Where(d=>d.State==true).Include(d => d.IdAuthorNavigation).Include(d => d.Responses).OrderByDescending(d => d.CreationDate);

            if (!string.IsNullOrEmpty(title))
                discussions = discussions.Where(d => d.Title.Contains(title));

            return View(await discussions.ToListAsync());
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
    }
}

