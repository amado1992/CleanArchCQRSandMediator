using AutoMapper;
using CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogs;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogById
{
    public class GetBlogByIdQueryHandler : IRequestHandler<GetBlogByIdQuery, BlogVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetBlogByIdQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<BlogVm> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
        {
            var blog = await _context.Blogs.AsTracking()
               .FirstOrDefaultAsync(model => model.Id == request.BlogId);

            return _mapper.Map<BlogVm>(blog);
        }
    }
}
