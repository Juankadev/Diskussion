using Diskussion.Models;
using Diskussion.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Diskussion.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DiskussionDbContext context) : base(context) 
        { 
        }
       
    }
}
