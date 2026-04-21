using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchCQRSandMediator.infra.Data
{
    public class BlogDbContext : DbContext, IApplicationDbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
    }
}
