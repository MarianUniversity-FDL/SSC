using SSC.Models;

namespace SSC.Services.Interfaces;

public interface ISCVService
{
    Task<string> GetRegistrarIdAsync(string peopleCodeId);
    Task<List<SCVSearch>> GetSearchResultsAsync(string peopleCodeId,string searchString);
    Task<List<ImagePath>> GetImagePathsAsync(string documentId);
}
