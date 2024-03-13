using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SSC.Models;
using SSC.Services.Interfaces;
using SSC.ViewModels;
using System.Diagnostics;

namespace SSC.Controllers;

public class ASRController : Controller
{
    private readonly IASRService _asrService;

    public ASRController(IASRService asrService)
    {
        _asrService=asrService;
    }

    [HttpGet]
    public async Task<IActionResult> ASRSearch(string peopleCodeId)
    {
        if (string.IsNullOrWhiteSpace(peopleCodeId))
        {
            return View("Error",new ErrorViewModel { RequestId=Activity.Current?.Id??HttpContext.TraceIdentifier });
        }

        var instructorDetails = await _asrService.GetInstructorInformationAsync(peopleCodeId);
        var viewModel = new ASRSearchViewModel
        {
            InstructorDetails=instructorDetails,
            StudentCourses=new List<StudentCourse>()
        };

        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> ASRSearch(string peopleCodeId,string searchString)
    {
        if (string.IsNullOrWhiteSpace(peopleCodeId))
        {
            return View("Error",new ErrorViewModel { RequestId=Activity.Current?.Id??HttpContext.TraceIdentifier });
        }

        var instructorDetails = await _asrService.GetInstructorInformationAsync(peopleCodeId);
        var studentCourses = await _asrService.GetStudentCourseInfoAsync(peopleCodeId,searchString);

        var viewModel = new ASRSearchViewModel
        {
            InstructorDetails=instructorDetails,
            StudentCourses=studentCourses
        };

        return View(viewModel);
    }

    public IActionResult SelectCourse(string peopleCodeId,string instructorName,string semester,string studentId,string studentName,
        string courseNumber,string courseName,string courseType,string section,string sectionId)
    {
        var courseDetails = new CourseDetail
        {
            InstructorId=peopleCodeId,
            InstructorName=instructorName,
            Semester=semester,
            StudentId=studentId,
            StudentName=studentName,
            CourseNumber=courseNumber,
            CourseName=courseName,
            CourseType=courseType,
            Section=section,
            SectionId=sectionId
        };

        TempData["CourseDetails"]=JsonConvert.SerializeObject(courseDetails);
        return RedirectToAction("ASRForm");
    }

    public IActionResult ASRForm()
    {
        var courseDetailsJson = TempData["CourseDetails"] as string;
        TempData.Keep("CourseDetails");
        var courseDetails = JsonConvert.DeserializeObject<CourseDetail>(courseDetailsJson);

        var courseInformation = new CourseDetail
        {
            InstructorId=courseDetails.InstructorId,
            InstructorName=courseDetails.InstructorName,
            Semester=courseDetails.Semester,
            StudentId=courseDetails.StudentId,
            StudentName=courseDetails.StudentName,
            CourseNumber=courseDetails.CourseNumber,
            CourseName=courseDetails.CourseName,
            CourseType=courseDetails.CourseType,
            Section=courseDetails.Section,
            SectionId=courseDetails.SectionId
        };

        var selectListModel = new Models.SelectList
        {
            CurrentGradeOptions=new List<SelectListItem>
            {
                new SelectListItem("A", "A"),
                new SelectListItem("A-", "A-"),
                new SelectListItem("B+", "B+"),
                new SelectListItem("B", "B"),
                new SelectListItem("B-", "B-"),
                new SelectListItem("C+", "C+"),
                new SelectListItem("C", "C"),
                new SelectListItem("C-", "C-"),
                new SelectListItem("D+", "D+"),
                new SelectListItem("D", "D"),
                new SelectListItem("D-", "D-"),
                new SelectListItem("F", "F"),
            },
            AttendanceConcernOptions=new List<SelectListItem>
            {
                new SelectListItem("Always","Always"),
                new SelectListItem("Usually","Usually"),
                new SelectListItem("Sometimes","Sometimes"),
                new SelectListItem("Rarely","Rarely"),
                new SelectListItem("Never","Never")
            },
            EngagementConcernOptions=new List<SelectListItem>
            {
                new SelectListItem("Not Concerned","Not Concerned"),
                new SelectListItem("Slightly","Slightly Concerned"),
                new SelectListItem("Moderately","Moderately Concerned"),
                new SelectListItem("Highly","Highly Concerned"),
                new SelectListItem("Extremely","Extremely Concerned")
            },
            HomeworkSubmittedOptions=new List<SelectListItem>
            {
                new SelectListItem("Always","Always"),
                new SelectListItem("Usually","Usually"),
                new SelectListItem("Sometimes","Sometimes"),
                new SelectListItem("Rarely","Rarely"),
                new SelectListItem("Never","Never"),
                new SelectListItem("Not Available","Not Available")
            },
            NonCognitiveConcernOptions=new List<SelectListItem>
            {
                new SelectListItem("Yes", "Yes"),
                new SelectListItem("No", "No"),
                new SelectListItem("Maybe", "Maybe")
            }
        };

        var radioButtonModel = new RadioButton
        {
            StudentNotifiedASR=false,
            StudentConferenceArranged=false,
            ReferalEnglish=false,
            ReferalMath=false,
            ReferalScience=false,
            ReferalNursing=false
        };

        var asrFormViewModel = new ASRFormViewModel
        {
            SelectListModel=selectListModel,
            CourseDetail=courseInformation,
            RadioButtonModel=radioButtonModel,
            CommentModel=new Comment()
        };

        return View(asrFormViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitASRForm(ASRFormSubmit asrSubmission)
    {
        if (ModelState.IsValid)
        {
            try
            {
                bool isSuccess = await _asrService.InsertASRRecord(asrSubmission);
                if (isSuccess)
                {
                    ViewData["Message"]="Submission was successful.";
                    ViewData["InstructorId"]=asrSubmission.InstructorId;
                }
                else
                {
                    ViewData["Message"]="Submission failed.";
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"]=$"Error: {ex.Message}";
            }
            return View("ASRSubmission");
        }

        ViewData["Message"]="Error: Model State is Invalid!";
        return View("ASRSubmission");
    }
}
