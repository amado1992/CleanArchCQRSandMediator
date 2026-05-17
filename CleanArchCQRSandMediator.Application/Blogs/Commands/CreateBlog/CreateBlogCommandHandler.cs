using AutoMapper;
using CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogs;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Domain.Entity;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Blogs.Commands.CreateBlog
{
    public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, BlogVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CreateBlogCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<BlogVm> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            var blogEntity = new Blog() 
            { 
                Name = request.Name,
                Description = request.Description,
                Author = request.Author 
            };

            await _context.Blogs.AddAsync(blogEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<BlogVm>(blogEntity);
        }
    }
}
