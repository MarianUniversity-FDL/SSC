using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SSC.Models;

public class SelectList
{
    [Required(ErrorMessage = "Please select an option.")]
    public string CurrentGrade { get; set; }
    public IEnumerable<SelectListItem>? CurrentGradeOptions { get; set; }

    [Required(ErrorMessage = "Please select an option.")]
    public string AttendanceConcern { get; set; }
    public IEnumerable<SelectListItem>? AttendanceConcernOptions { get; set; }

    [Required(ErrorMessage = "Please select an option.")]
    public string EngagementConcern { get; set; }
    public IEnumerable<SelectListItem>? EngagementConcernOptions { get; set; }

    [Required(ErrorMessage = "Please select an option.")]
    public string HomeworkSubmitted { get; set; }
    public IEnumerable<SelectListItem>? HomeworkSubmittedOptions { get; set; }

    [Required(ErrorMessage = "Please select an option.")]
    public string NonCognitiveConcern { get; set; }
    public IEnumerable<SelectListItem>? NonCognitiveConcernOptions { get; set; }

}
