using Dapper;
using SSC.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace SSC.Services;

public class NSCHService : INSCHService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public NSCHService(HttpClient httpClient,IConfiguration configuration)
    {
        _httpClient=httpClient;
        _configuration=configuration;
    }

    public async Task<string> GetRedirectUrlAsync(string peopleCodeId)
    {
        var userId = _configuration["NSCHCredentials:UserId"];
        var password = _configuration["NSCHCredentials:Password"];
        var url = "https://secureapi.studentclearinghouse.org/sssportalui/authenticate";
        var referer = "https://muxss5.marianuniversity.edu/powercampusselfservice/nch/nch.aspx";

        var SSN = await GetStudentSSN(peopleCodeId);

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("user_id", userId),
            new KeyValuePair<string, string>("password", password),
            new KeyValuePair<string, string>("qu", SSN)
        });

        _httpClient.DefaultRequestHeaders.Referrer=new Uri(referer);
        var response = await _httpClient.PostAsync(url,content);

        if (response.IsSuccessStatusCode)
        {
            var responseUri = response.RequestMessage.RequestUri.AbsoluteUri;
            return responseUri;
        }
        else
        {
            throw new HttpRequestException($"Request to NSCH failed with status code {response.StatusCode}.");
        }
    }

    public async Task<string> GetStudentSSN(string peopleCodeId)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("PowerCampus"));
        var result = await connection.QuerySingleOrDefaultAsync<string>(
            "MU_sp_NSCH_GetStudentSSN",
            new { PeopleCodeId = peopleCodeId },
            commandType: CommandType.StoredProcedure);

        return result??throw new KeyNotFoundException("Student Data Not Found");
    }
}