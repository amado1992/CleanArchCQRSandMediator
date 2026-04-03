using AutoMapper;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogs
{
    public class GetBlogQueryHandler : IRequestHandler<GetBlogQuery, List<BlogVm>>
    {
        private readonly IMapper _mapper;

        public GetBlogQueryHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<List<BlogVm>> Handle(GetBlogQuery request, CancellationToken cancellationToken)
        {
            // var blogList = _mapper.Map<List<BlogVm>>(blogs);
            throw new NotImplementedException();
        }
    }
}
