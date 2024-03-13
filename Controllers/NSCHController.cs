using Microsoft.AspNetCore.Mvc;
using SSC.Services.Interfaces;

namespace SSC.Controllers;

public class NSCHController : Controller
{
    private readonly INSCHService _nschService;

    public NSCHController(INSCHService nschService)
    {
        _nschService=nschService;
    }

    public IActionResult NSCHPortal()
    {
        return View();
    }


    // use if NSCH API user/pw is active and valid. 
    //
    //[HttpGet]
    //public async Task<IActionResult> NSCHPortal(string peopleCodeId)
    //{
    //    if (string.IsNullOrWhiteSpace(peopleCodeId))
    //    {
    //        return View("Error",new ErrorViewModel { RequestId=Activity.Current?.Id??HttpContext.TraceIdentifier });

    //    }

    //    ViewBag.PeopleCodeId=peopleCodeId;
    //    return View();
    //}

    //public async Task<IActionResult> RedirectUser(string peopleCodeId)
    //{
    //    try
    //    {
    //        var redirectUrl = await _nschService.GetRedirectUrlAsync(peopleCodeId);
    //        return Redirect(redirectUrl);
    //    }
    //    catch (Exception ex)
    //    {
    //        return RedirectToAction("Error","Home");
    //    }
    //}
}