using CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogs;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Blogs.Commands.CreateBlog
{
    public class CreateBlogCommand : IRequest<BlogVm>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }
}
