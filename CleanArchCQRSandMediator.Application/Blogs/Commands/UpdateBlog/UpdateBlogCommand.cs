using CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogs;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Blogs.Commands.UpdateBlog
{
    public record UpdateBlogCommand : IRequest<BlogVm> // IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
    }
}
