namespace CleanArchCQRSandMediator.Application.Dtos.Permissions
{
    public class PermissionsResponse
    {
        public IList<string> Permissions { get; set; } = new List<string>();
    }
}
