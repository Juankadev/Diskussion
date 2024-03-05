using Diskussion.Models;
using Diskussion.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Diskussion.Repositories
{
    public class DiscussionRepository : GenericRepository<Discussion>, IDiscussionRepository
    {
        public DiscussionRepository(DiskussionDbContext context) : base(context)
        {
        }

        public async Task<List<Discussion>> GetAllByIdWithResponses(long? id)
        {
            return await GetAll(d => d.IdAuthor == id).Include(d => d.Responses).OrderByDescending(d => d.CreationDate).ToListAsync();
        }

        public List<Discussion> GetAllByTitle(string title)
        {
            IQueryable<Discussion> discussions = GetAll().Include(d => d.IdAuthorNavigation).Include(d => d.Responses).OrderByDescending(d => d.CreationDate);

            if (!string.IsNullOrEmpty(title))
                discussions = discussions.Where(d => d.Title.Contains(title));

            return discussions.ToList();
        }

        public Discussion GetByIdWithIncludes(long id)
        {
            return GetAll(d=>d.Id == id)
                   .Include(d => d.IdAuthorNavigation) // Incluir al autor de la discusión
                   .Include(d => d.Responses) // Incluir las respuestas de la discusión
                        .ThenInclude(r => r.IdAuthorNavigation) // Incluir al usuario asociado a cada respuesta
                   .First(d => d.Id == id);
        }
    }
}
