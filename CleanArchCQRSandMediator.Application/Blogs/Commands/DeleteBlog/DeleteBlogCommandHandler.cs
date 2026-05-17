using CleanArchCQRSandMediator.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchCQRSandMediator.Application.Blogs.Commands.DeleteBlog
{
    public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommand, int>
    {
        private readonly IApplicationDbContext _context;
        public DeleteBlogCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
        {
            return await _context.Blogs
                .Where(model => model.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
