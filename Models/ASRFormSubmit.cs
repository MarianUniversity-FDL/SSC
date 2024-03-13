namespace SSC.Models;

public class ASRFormSubmit
{
    public string InstructorId { get; set; }
    public string InstructorName { get; set; }
    public string StudentId { get; set; }
    public string CourseNumber { get; set; }
    public string CourseName { get; set; }
    public string SectionId { get; set; }
    public SelectList SelectListModel { get; set; }
    public RadioButton RadioButtonModel { get; set; }
    public Comment CommentModel { get; set; }
}
