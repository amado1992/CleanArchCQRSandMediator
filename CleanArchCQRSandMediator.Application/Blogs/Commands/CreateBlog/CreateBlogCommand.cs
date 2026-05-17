using CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogs;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Blogs.Commands.CreateBlog
{
    public class CreateBlogCommand : IRequest<BlogVm>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }
}
