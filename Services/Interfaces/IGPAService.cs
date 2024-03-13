using SSC.Models;

namespace SSC.Services.Interfaces;

public interface IGPAService
{
    Task<IEnumerable<StudentCourseHistory>> GetStudentCourseHistoryAsync(string peopleCodeId);
}