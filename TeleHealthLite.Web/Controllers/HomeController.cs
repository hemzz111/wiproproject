using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TeleHealthLite.Web.Models;

namespace TeleHealthLite.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        int visitCount = HttpContext.Session.GetInt32("HomeVisitCount") ?? 0;

        visitCount++;

        HttpContext.Session.SetInt32("HomeVisitCount", visitCount);

        ViewBag.VisitCount = visitCount;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}