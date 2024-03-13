using Microsoft.AspNetCore.Mvc;
using SSC.Services.Interfaces;
using SSC.ViewModels;
using System.Diagnostics;

namespace SSC.Controllers;

public class GPAController : Controller
{
    private readonly IGPAService _gpaService;

    public GPAController(IGPAService gpaService)
    {
        _gpaService=gpaService;
    }

    [HttpGet]
    public async Task<IActionResult> GPACalculator(string peopleCodeId)
    {
        if (string.IsNullOrWhiteSpace(peopleCodeId))
        {
            return View("Error",new ErrorViewModel { RequestId=Activity.Current?.Id??HttpContext.TraceIdentifier });
        }

        var studentCourseHistory = await _gpaService.GetStudentCourseHistoryAsync(peopleCodeId);

        return View(studentCourseHistory);
    }
}