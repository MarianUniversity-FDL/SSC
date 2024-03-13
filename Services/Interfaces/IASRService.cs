using SSC.Models;

namespace SSC.Services.Interfaces;

public interface IASRService
{
    Task<InstructorDetail> GetInstructorInformationAsync(string peopleCodeId);
    Task<List<StudentCourse>> GetStudentCourseInfoAsync(string peopleCodeId, string searchString);
    Task<bool> InsertASRRecord(ASRFormSubmit formSubmission);
}