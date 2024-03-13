using Dapper;
using SSC.Models;
using SSC.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace SSC.Services;

public class GPAService : IGPAService
{
    private readonly IConfiguration _configuration;

    public GPAService(IConfiguration configuration)
    {
        _configuration=configuration;
    }

    public async Task<IEnumerable<StudentCourseHistory>> GetStudentCourseHistoryAsync(string peopleCodeId)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("PowerCampus"));
        var result = await connection.QueryAsync<StudentCourseHistory>(
            "MU_sp_GPA_GetStudentCourseHistory",
            new { PeopleCodeId = peopleCodeId },
            commandType: CommandType.StoredProcedure);

        return result.ToList();
    }
}