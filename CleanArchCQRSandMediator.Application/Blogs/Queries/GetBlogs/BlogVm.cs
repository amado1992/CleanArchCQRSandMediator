using AutoMapper;
using CleanArchCQRSandMediator.Application.Common.Mappings;
using CleanArchCQRSandMediator.Domain.Entities.Business;

namespace CleanArchCQRSandMediator.Application.Blogs.Queries.GetBlogs
{
    public class BlogVm : IMapFrom<Blog>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Blog, BlogVm>();
        }
    }
}
