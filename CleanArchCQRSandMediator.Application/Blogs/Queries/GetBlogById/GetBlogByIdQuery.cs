using CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogs;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogById
{
    public class GetBlogByIdQuery : IRequest<BlogVm>
    {
        public int BlogId { get; set; }
    }
}
