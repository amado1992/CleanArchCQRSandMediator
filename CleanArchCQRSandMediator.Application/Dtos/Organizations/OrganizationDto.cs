using AutoMapper;
using CleanArchCQRSandMediator.Application.Common.Mappings;
using CleanArchCQRSandMediator.Domain.Entities.Business;

namespace CleanArchCQRSandMediator.Application.Dtos.Organizations
{
    public class OrganizationDto : IMapFrom<Tenant>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool IsActive
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Tenant, OrganizationDto>();
        }
    }
}
