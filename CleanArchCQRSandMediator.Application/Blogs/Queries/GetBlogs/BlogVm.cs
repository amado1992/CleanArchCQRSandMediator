using AutoMapper;
using CleanArchCQRSandMediator.Application.Common.Mappings;
using CleanArchCQRSandMediator.Domain.Entities.Business;

namespace CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogs
{
    public class BlogVm : IMapFrom<Blog>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Blog, BlogVm>();
        }
    }
}
