namespace CleanArchCQRSandMediator.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        public int GetUserId();
        public string? GetUserEmail();
        public string? GetFullName();
        public IList<string> GetRoles();
        public bool IsAuthenticated();
        public bool HasRole(string roleName);
    }
}
