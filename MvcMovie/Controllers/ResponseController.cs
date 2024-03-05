using Diskussion.Models;
using Diskussion.Repositories;
using Diskussion.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Diskussion.Controllers
{
    public class ResponseController : Controller
    {
        private readonly IResponseRepository _responseRepository;
        public ResponseController(IResponseRepository responseRepository)
        {
            _responseRepository = responseRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Response response)
        {
            if (string.IsNullOrEmpty(response.Message))
                return RedirectToAction(nameof(Discussion), new { id = response.IdDiscussion });

            var newResponse = new Response()
            {
                IdAuthor = long.Parse(HttpContext.Session.GetString("User_Id")),
                IdDiscussion = response.IdDiscussion,
                Message = response.Message,
            };

            await _responseRepository.Insert(newResponse);
            await _responseRepository.Save();

            return RedirectToAction("Open", nameof(Discussion), new { id = response.IdDiscussion });
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var response = await _responseRepository.GetById(id);

            if (response == null) return NotFound();

            await _responseRepository.HardDelete(id);
            await _responseRepository.Save();

            return RedirectToAction("Open", nameof(Discussion), new { id = response.IdDiscussion });
        }
    }
}
