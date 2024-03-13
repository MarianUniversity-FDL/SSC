using Dapper;
using SSC.Models;
using SSC.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace SSC.Services;

public class SCVService : ISCVService
{
    private readonly IConfiguration _configuration;
    public SCVService(IConfiguration configuration)
    {
        _configuration=configuration;
    }

    public async Task<string> GetRegistrarIdAsync(string peopleCodeId)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("PowerCampus"));
        var result = await connection.QuerySingleOrDefaultAsync<string>(
            "MU_sp_SCV_GetRegistrarId",
            new { PeopleCodeId = peopleCodeId },
            commandType: CommandType.StoredProcedure);

        return result??throw new KeyNotFoundException("Registrar Not Found");
    }

    public async Task<List<SCVSearch>> GetSearchResultsAsync(string peopleCodeId,string searchString)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("PowerCampus"));
        var result = await connection.QueryAsync<SCVSearch>(
            "MU_sp_SCV_GetSearchResults",
            new { PeopleCodeId = peopleCodeId,SearchString = searchString },
            commandType: CommandType.StoredProcedure);

        return result.ToList();
    }

    public async Task<List<ImagePath>> GetImagePathsAsync(string documentId)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("PowerCampus"));
        var result = await connection.QueryAsync<ImagePath>(
            "MU_sp_SCV_GetImagePaths",
            new { DocumentId = documentId },
            commandType: CommandType.StoredProcedure);

        return result.ToList();
    }
}
