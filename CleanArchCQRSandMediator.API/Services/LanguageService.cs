using System.Globalization;
using CleanArchCQRSandMediator.Application.Common.Interfaces;

public class LanguageService : ILanguageService
{
    public string GetCurrentLanguage()
    {
        return CultureInfo.CurrentUICulture.Name;
    }
}