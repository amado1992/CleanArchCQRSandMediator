using CleanArchCQRSandMediator.Domain.Entities.Business;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CleanArchCQRSandMediator.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public Task<int> SaveChangesAsync();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        // Both methods do the same thing
        public void SaveChangesSynchronous();
        public void SaveChanges();
    }
}
