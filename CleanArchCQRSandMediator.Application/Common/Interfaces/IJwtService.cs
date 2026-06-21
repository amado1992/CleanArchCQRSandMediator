namespace CleanArchCQRSandMediator.Application.Common.Interfaces
{
    /// <summary>
    /// Returns the token ID
    /// </summary>
    public interface IJwtService
    {
        string GetJtiFromToken(string token);
    }
}
