using MediatR;

namespace CleanArchCQRSandMediator.Application.Organizations.Commands.CreateOrganization
{
    public class CreateOrganizationCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
