namespace SSC.Models;
public class StudentCourseHistory
{
    public string PeopleCodeId { get; set; }
    public string StudentName { get; set; }
    public string Program { get; set; }
    public string Degree { get; set; }
    public string Curriculum { get; set; }
    public string TranscriptSequence { get; set; }
    public int AcademicYear { get; set; }
    public string AcademicTerm { get; set; }
    public string CourseId { get; set; }
    public string CourseName { get; set; }
    public string CourseType { get; set; }
    public string CreditType { get; set; }
    public bool CourseRepeated { get; set; }
    public bool AffectsGPA { get; set; }
    public string Grade { get; set; }
    public decimal Credit { get; set; }
    public decimal CourseGPA { get; set; }
    public int SemesterSequence { get; set; }
}