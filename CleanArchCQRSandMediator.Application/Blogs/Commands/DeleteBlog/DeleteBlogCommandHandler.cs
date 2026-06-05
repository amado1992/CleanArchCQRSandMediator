using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Domain.Entities.Business;
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

        /// <summary>
        /// Bulk delete
        /// </summary>

        /*public async Task<int> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
        {
            return await _context.Blogs
                .Where(model => model.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);
        }*/

        /// <summary>
        /// Tracked entity (recommended)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<int> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
        {
            var blog = await _context.Blogs.FindAsync(request.Id, cancellationToken);
            if (blog is null) throw new NotFoundException(nameof(Blog), request.Id);

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync(cancellationToken);

            return request.Id; // o 0, o void
        }
    }
}
