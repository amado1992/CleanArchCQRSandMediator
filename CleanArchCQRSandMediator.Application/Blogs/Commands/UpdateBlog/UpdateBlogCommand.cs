using CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogs;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Blogs.Commands.UpdateBlog
{
    public record UpdateBlogCommand : IRequest<BlogVm> // IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }
}
