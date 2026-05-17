using CleanArchCQRSandMediator.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchCQRSandMediator.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public Task<int> SaveChangesAsync();

        // Both methods do the same thing
        public void SaveChangesSynchronous();
        public void SaveChanges();
    }
}
