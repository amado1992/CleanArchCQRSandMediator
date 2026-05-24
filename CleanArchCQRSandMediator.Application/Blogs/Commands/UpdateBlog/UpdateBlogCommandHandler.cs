using AutoMapper;
using CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogs;
using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchCQRSandMediator.Application.Blogs.Commands.UpdateBlog
{
    public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommand, BlogVm> // IRequestHandler<UpdateBlogCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateBlogCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        /*public async Task<int> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
        {
            var blog = new Blog()
            {
                // Id = request.Id,
                Author = request.Author,
                Description = request.Description,
                Name = request.Name
            };

            return await _context.Blogs
                .Where(model => model.Id == request.Id)
                .ExecuteUpdateAsync(setters => setters
                // .SetProperty(m => m.Id, blog.Id)
                .SetProperty(m => m.Name, blog.Name)
                .SetProperty(m => m.Description, blog.Description)
                .SetProperty(m => m.Author, blog.Author),
                cancellationToken);
        }*/

        /*public async Task<int> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
        {
            var blog = await _context.Blogs.FindAsync(request.Id);
            if (blog is null) throw new NotFoundException();

            blog.Name = request.Name;
            blog.Description = request.Description;
            blog.Author = request.Author;

            await _context.SaveChangesAsync();
            return blog.Id;
        }*/

        public async Task<BlogVm> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
        {
            var blog = await _context.Blogs.FindAsync(new object[] { request.Id }, cancellationToken);
            if (blog is null) throw new NotFoundException(nameof(Blog), request.Id);

            blog.Name = request.Name;
            blog.Description = request.Description;
            blog.Author = request.Author;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<BlogVm>(blog);
        }
    }
}
