using SSC.Models;

namespace SSC.ViewModels;

public class ASRFormViewModel
{
    public SelectList? SelectListModel { get; set; }
    public CourseDetail? CourseDetail { get; set; }
    public RadioButton? RadioButtonModel { get; set; }
    public Comment? CommentModel { get; set; }
}
