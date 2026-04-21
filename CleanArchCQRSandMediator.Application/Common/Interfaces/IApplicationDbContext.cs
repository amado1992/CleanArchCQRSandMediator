using CleanArchCQRSandMediator.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchCQRSandMediator.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Blog> Blogs { get; set; }
    }
}
