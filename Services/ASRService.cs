using Dapper;
using SSC.Models;
using SSC.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace SSC.Services;
public class ASRService : IASRService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ASRService> _logger;

    public ASRService(IConfiguration configuration,ILogger<ASRService> logger)
    {
        _configuration=configuration;
        _logger=logger;
    }

    public async Task<InstructorDetail> GetInstructorInformationAsync(string peopleCodeId)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("PowerCampus"));
        var result = await connection.QuerySingleOrDefaultAsync<InstructorDetail>(
            "MU_sp_ASR_GetInstructorInformation",
            new { PeopleCodeId = peopleCodeId },
            commandType: CommandType.StoredProcedure);

        return result??throw new KeyNotFoundException("Instructor Not Found");
    }

    public async Task<List<StudentCourse>> GetStudentCourseInfoAsync(string peopleCodeId,string searchString)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("PowerCampus"));
        var result = await connection.QueryAsync<StudentCourse>(
            "MU_sp_ASR_GetStudentCourseInfo",
            new { PeopleCodeId = peopleCodeId,SearchString = searchString },
            commandType: CommandType.StoredProcedure);

        return result.ToList();
    }

    public async Task<bool> InsertASRRecord(ASRFormSubmit asrSubmission)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("PowerCampus")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("@InstructorName",asrSubmission.InstructorName,DbType.String);
            parameters.Add("@StudentId",asrSubmission.StudentId,DbType.String);
            parameters.Add("@CourseNumber",asrSubmission.CourseNumber,DbType.String);
            parameters.Add("@CourseName",asrSubmission.CourseName,DbType.String);
            parameters.Add("@SectionId",asrSubmission.SectionId,DbType.String);
            parameters.Add("@CurrentGrade",asrSubmission.SelectListModel.CurrentGrade,DbType.String);
            parameters.Add("@HomeworkSubmitted",asrSubmission.SelectListModel.HomeworkSubmitted,DbType.String);
            parameters.Add("@EngagementConcern",asrSubmission.SelectListModel.EngagementConcern,DbType.String);
            parameters.Add("@AttendanceConcern",asrSubmission.SelectListModel.AttendanceConcern,DbType.String);
            parameters.Add("@NonCognitiveConcern",asrSubmission.SelectListModel.NonCognitiveConcern,DbType.String);
            parameters.Add("@StudentNotifiedASR",asrSubmission.RadioButtonModel.StudentNotifiedASR,DbType.String);
            parameters.Add("@StudentConferenceArranged",asrSubmission.RadioButtonModel.StudentConferenceArranged,DbType.String);
            parameters.Add("@ReferalEnglish",asrSubmission.RadioButtonModel.ReferalEnglish,DbType.String);
            parameters.Add("@ReferalMath",asrSubmission.RadioButtonModel.ReferalMath,DbType.String);
            parameters.Add("@ReferalScience",asrSubmission.RadioButtonModel.ReferalScience,DbType.String);
            parameters.Add("@ReferalNursing",asrSubmission.RadioButtonModel.ReferalNursing,DbType.String);
            parameters.Add("@Comments",asrSubmission.CommentModel.Comments,DbType.String);
            parameters.Add("@Result",dbType: DbType.Boolean,direction: ParameterDirection.Output);

            try
            {
                connection.Open();
                await connection.ExecuteAsync("MU_sp_ASR_InsertRecord",parameters,commandType: CommandType.StoredProcedure);
                bool result = parameters.Get<bool>("@Result");

                if (!result)
                {
                    throw new Exception("Record insertion failed.");
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error Inserting ASR Record");
                throw;
            }
        }
    }
}