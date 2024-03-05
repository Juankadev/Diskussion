using Diskussion.Models;
using Diskussion.Repositories.Interfaces;

namespace Diskussion.Repositories
{
    public class ResponseRepository : GenericRepository<Response>, IResponseRepository
    {
        public ResponseRepository(DiskussionDbContext context) : base(context)
        {
        }
    }
}
