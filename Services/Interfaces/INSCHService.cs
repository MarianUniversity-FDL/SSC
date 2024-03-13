namespace SSC.Services.Interfaces;

public interface INSCHService
{
    Task<string> GetRedirectUrlAsync(string peopleCodeId);
}