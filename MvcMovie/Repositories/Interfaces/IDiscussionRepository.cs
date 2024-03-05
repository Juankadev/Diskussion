using Diskussion.Models;

namespace Diskussion.Repositories.Interfaces
{
    public interface IDiscussionRepository : IGenericRepository<Discussion>
    {
        Task<List<Discussion>> GetAllByIdWithResponses(long? id);
        List<Discussion> GetAllByTitle(string title);
        Discussion GetByIdWithIncludes(long id);
    }
}
