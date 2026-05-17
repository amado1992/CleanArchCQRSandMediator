using MediatR;

namespace CleanArchCQRSandMediator.Application.Blogs.Commands.DeleteBlog
{
    public record DeleteBlogCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
