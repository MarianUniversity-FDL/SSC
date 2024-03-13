using Microsoft.AspNetCore.Mvc;
using SSC.Models;
using SSC.Services.Interfaces;
using SSC.ViewModels;
using System.Diagnostics;

namespace SSC.Controllers;

public class SCVController : Controller
{
    private readonly ISCVService _scvService;

    public SCVController(ISCVService scvService)
    {
        _scvService=scvService;
    }

    [HttpGet]
    public async Task<IActionResult> SCVSearch(string peopleCodeId)
    {
        if (string.IsNullOrWhiteSpace(peopleCodeId))
        {
            return View("Error",new ErrorViewModel { RequestId=Activity.Current?.Id??HttpContext.TraceIdentifier });
        }

        var PeopleCodeId = await _scvService.GetRegistrarIdAsync(peopleCodeId);
        var viewModel = new SCVViewModel
        {
            PeopleCodeId=PeopleCodeId,
            SearchResults=new List<SCVSearch>()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SCVSearch(string peopleCodeId,string searchString)
    {
        if (string.IsNullOrWhiteSpace(peopleCodeId))
        {
            return View("Error",new ErrorViewModel { RequestId=Activity.Current?.Id??HttpContext.TraceIdentifier });
        }

        var PeopleCodeId = await _scvService.GetRegistrarIdAsync(peopleCodeId);
        var searchResults = await _scvService.GetSearchResultsAsync(peopleCodeId,searchString);

        var viewModel = new SCVViewModel
        {
            PeopleCodeId=PeopleCodeId,
            SearchResults=searchResults
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> SCViewer(string documentId)
    {
        if (string.IsNullOrWhiteSpace(documentId))
        {
            return View("Error",new ErrorViewModel { RequestId=Activity.Current?.Id??HttpContext.TraceIdentifier });
        }

        var documentFilePath = await _scvService.GetImagePathsAsync(documentId);

        var viewModel = new SCVImageViewModel
        {
            DocumentFilePath=documentFilePath
        };

        return View(viewModel);
    }

    public async Task<IActionResult> GetImage(string imagePath)
    {
        var fullPath = imagePath;
        Console.WriteLine($"Attempting to serve image from path: {fullPath}"); // For debugging

        if (System.IO.File.Exists(fullPath))
        {
            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
            return File(fileBytes,"image/jpeg");
        }
        Console.WriteLine($"File not found at path: {fullPath}"); // For debugging
        return NotFound();
    }

}
