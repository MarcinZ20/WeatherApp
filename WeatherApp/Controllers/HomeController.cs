using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Models;

namespace WeatherApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewData["Warsaw"] = Database_controller.ListRecords()[0].Temp;
        ViewData["Paris"] = Database_controller.ListRecords()[1].Temp;
        ViewData["NewYork"] = Database_controller.ListRecords()[2].Temp;
        ViewData["Tokyo"] = Database_controller.ListRecords()[3].Temp;

        ViewData["Hourly"] = Database_controller.ListHourly();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Login()
    {
        return ViewBag.Login;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

