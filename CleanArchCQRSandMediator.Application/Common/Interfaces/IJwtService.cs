namespace CleanArchCQRSandMediator.Application.Common.Interfaces
{
    /// <summary>
    /// Returns the token ID
    /// </summary>
    public interface IJwtService
    {
        public string GetJtiFromToken(string token);
    }
}
