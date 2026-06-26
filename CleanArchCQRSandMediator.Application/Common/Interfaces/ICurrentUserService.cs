namespace CleanArchCQRSandMediator.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        public int GetUserId();
        public string? GetUserEmail();
        public bool IsAuthenticated();
    }
}
