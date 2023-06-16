using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Areas.Identity.Data;
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
        ViewData["Warsaw"] = ParseTemperature(Database_controller.ListRecords()[0].Temp);
        ViewData["Paris"] = ParseTemperature(Database_controller.ListRecords()[1].Temp);
        ViewData["NewYork"] = ParseTemperature(Database_controller.ListRecords()[2].Temp);
        ViewData["Tokyo"] = ParseTemperature(Database_controller.ListRecords()[3].Temp);

        ViewData["Wfeels"] = ParseTemperature(Database_controller.ListRecords()[0].Feels);
        ViewData["Pfeels"] = ParseTemperature(Database_controller.ListRecords()[1].Feels);
        ViewData["Nfeels"] = ParseTemperature(Database_controller.ListRecords()[2].Feels);
        ViewData["Tfeels"] = ParseTemperature(Database_controller.ListRecords()[3].Feels);

        ViewData["Whum"] = ParseTemperature(Database_controller.ListRecords()[0].Humidity);
        ViewData["Phum"] = ParseTemperature(Database_controller.ListRecords()[1].Humidity);
        ViewData["Nhum"] = ParseTemperature(Database_controller.ListRecords()[2].Humidity);
        ViewData["Thum"] = ParseTemperature(Database_controller.ListRecords()[3].Humidity);

        ViewData["Wpress"] = ParseTemperature(Database_controller.ListRecords()[0].Pressure);
        ViewData["Ppress"] = ParseTemperature(Database_controller.ListRecords()[1].Pressure);
        ViewData["Npress"] = ParseTemperature(Database_controller.ListRecords()[2].Pressure);
        ViewData["Tpress"] = ParseTemperature(Database_controller.ListRecords()[3].Pressure);

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

    private static string ParseTemperature(string temp)
    {
        return Convert.ToInt64(double.Parse(temp.Replace('.', ','))).ToString();
    }
}

